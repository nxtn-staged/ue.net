// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "CoreClr.hpp"

#include "NextTurnRuntime.hpp"

#include <coreclr_delegates.h>
#include <hostfxr.h>
#include <nethost.h>

#include "Runtime/Core/Public/HAL/PlatformProcess.h"
#include "Runtime/Core/Public/Misc/Paths.h"
#include "Async/AsyncWork.h"
ENABLE_WARNINGS;

namespace Assert
{
	template<size_t N, typename... Types>
	void Fail(const TCHAR (&Format)[N], Types... Arguments)
	{
		RESTORE_WARNINGS;
		UE_LOG(LogNextTurnRuntime, Fatal, Format, Arguments...);
		ENABLE_WARNINGS;
	}

	template<size_t N, typename... Types>
	void NotNull(void* Ptr, const TCHAR (&Format)[N], Types... Arguments)
	{
		if (Ptr == nullptr)
		{
			Fail(Format, Arguments...);
		}
	}

	template<size_t N, typename... Types>
	void Succeeded(int32 Status, const TCHAR (&Format)[N], Types... Arguments)
	{
		if (Status < 0)
		{
			Fail(Format, Arguments...);
		}
	}
}

#if PLATFORM_WINDOWS
#define PALTEXT_PASTE(X) L##X
#else
#define PALTEXT_PASTE(X) u8##X
#endif
#define PALTEXT(X) PALTEXT_PASTE(X)

namespace Pal
{
	auto TCharToPalConv(const TCHAR* Str)
	{
#if PLATFORM_WINDOWS
		return StringCast<char_t>(Str);
#else
		return FTCHARToUTF8{ Str };
#endif
	}

	auto PalToTCharConv(const char_t* Str)
	{
#if PLATFORM_WINDOWS
		return StringCast<TCHAR>(Str);
#else
		return FUTF8ToTCHAR{ Str };
#endif
	}
}

namespace CoreClr
{
	void (STDCALL* FreeGCHandle_Method)(intptr_t Handle);

	void* (STDCALL *GetFunctionPointer_Method)(
		const char16_t* AssemblyName,
		int32_t AssemblyNameLength,
		const char16_t* TypeName,
		int32_t TypeNameLength,
		const char16_t* MethodName,
		int32_t MethodNameLength);

	void (STDCALL *Load_Method)();
	void (STDCALL *RegisterSymbols_Method)(const FTypeSymbol* Types, int32 TypesLength);
	int32_t (STDCALL *Unload_Method)();
	void (STDCALL *UnregisterSymbols_Method)(const FTypeSymbol* Types, int32 TypesLength);

	void FreeGCHandle(intptr_t Handle)
	{
		FreeGCHandle_Method(Handle);
	}

	void Initialize()
	{
		FString AssembliesDir{ TEXT("C:\\Users\\Liim\\Documents\\Unreal Projects\\MyProject\\Binaries\\UE.NET") };
		//FString AssembliesDir{ FPaths::ProjectDir() / TEXT("Binaries") / TEXT("UE.NET") };

		TArray<char_t> PathBuffer{};
		int32 PathBufferSize = FPlatformMisc::GetMaxPathLength();
		PathBuffer.AddUninitialized(PathBufferSize);
		size_t PathBufferSizeSize = static_cast<size_t>(PathBufferSize);

		auto AssembliesDirConv{ Pal::TCharToPalConv(*AssembliesDir) };
		get_hostfxr_parameters Parameters{ sizeof(get_hostfxr_parameters), AssembliesDirConv.Get(), nullptr };

		int32 Status = get_hostfxr_path(PathBuffer.GetData(), &PathBufferSizeSize, &Parameters);
		Assert::Succeeded(Status, TEXT("Failed to locate Host FXR: 0x%x"), Status);

		// Leak Host FXR for module reloading
		void* HostFxrLib = FPlatformProcess::GetDllHandle(Pal::PalToTCharConv(PathBuffer.GetData()).Get());
		Assert::NotNull(HostFxrLib, TEXT("Failed to load Host FXR"));

		auto HostFxrSetErrorWriter = reinterpret_cast<hostfxr_set_error_writer_fn>(
			FPlatformProcess::GetDllExport(HostFxrLib, TEXT("hostfxr_set_error_writer")));

		auto HostFxrInitialize = reinterpret_cast<hostfxr_initialize_for_runtime_config_fn>(
			FPlatformProcess::GetDllExport(HostFxrLib, TEXT("hostfxr_initialize_for_runtime_config")));

		auto HostFxrGetDelegate = reinterpret_cast<hostfxr_get_runtime_delegate_fn>(
			FPlatformProcess::GetDllExport(HostFxrLib, TEXT("hostfxr_get_runtime_delegate")));

		auto HostFxrClose = reinterpret_cast<hostfxr_close_fn>(
			FPlatformProcess::GetDllExport(HostFxrLib, TEXT("hostfxr_close")));

		check(HostFxrSetErrorWriter != nullptr);
		check(HostFxrInitialize != nullptr);
		check(HostFxrGetDelegate != nullptr);
		check(HostFxrClose != nullptr);

		HostFxrSetErrorWriter(
			[](const char_t* Message)
			{
				RESTORE_WARNINGS;
				UE_LOG(LogNextTurnRuntime, Error, TEXT("%s"), Pal::PalToTCharConv(Message).Get());
				ENABLE_WARNINGS;
			});

		hostfxr_handle ContextHandle;

		Status = HostFxrInitialize(
			Pal::TCharToPalConv(*(AssembliesDir / TEXT("NextTurn.UE.Loader.runtimeconfig.json"))).Get(),
			nullptr,
			&ContextHandle);

		//Status = HostFxrInitialize(
		//	Pal::TCharToPalConv(*(AssembliesDir / TEXT_STRINGIZE(UE_PROJECT_NAME) TEXT(".runtimeconfig.json"))).Get(),
		//	nullptr,
		//	&ContextHandle);

		Assert::Succeeded(Status, TEXT("Failed to initialize host context: 0x%x"), Status);

		load_assembly_and_get_function_pointer_fn CoreClrGetFunctionPointer;

		Status = HostFxrGetDelegate(
			ContextHandle,
			hostfxr_delegate_type::hdt_load_assembly_and_get_function_pointer,
			reinterpret_cast<void**>(&CoreClrGetFunctionPointer));

		Assert::Succeeded(Status, TEXT("Failed to initialize CoreCLR: 0x%x"), Status);

		Status = HostFxrClose(ContextHandle);
		Assert::Succeeded(Status, TEXT("Failed to close host context: 0x%x"), Status);

#define NEXTTURN_UE_LOADER "NextTurn.UE.Loader"

		FString LoaderAssemblyPath{ AssembliesDir / TEXT(NEXTTURN_UE_LOADER ".dll") };
		auto LoaderAssemblyPathConv{ Pal::TCharToPalConv(*LoaderAssemblyPath) };

		const char_t* LoaderTypeName = PALTEXT(NEXTTURN_UE_LOADER ", " NEXTTURN_UE_LOADER);

#define CORECLR_GET_FUNCTION_POINTER(MethodName, OutFunctionPointer) \
CoreClrGetFunctionPointer( \
	LoaderAssemblyPathConv.Get(), \
	LoaderTypeName, \
	PALTEXT(MethodName), \
	UNMANAGEDCALLERSONLY_METHOD, \
	nullptr, \
	reinterpret_cast<void**>(OutFunctionPointer));

		CORECLR_GET_FUNCTION_POINTER("FreeGCHandle", &FreeGCHandle_Method);
		CORECLR_GET_FUNCTION_POINTER("GetFunctionPointer", &GetFunctionPointer_Method);

		void (STDCALL *Initialize_Method)(
			const char16_t* ComponentAssemblyPath,
			int32 ComponentAssemblyPathLength,
			const FTypeSymbol* Types,
			int32 TypesLength);

		CORECLR_GET_FUNCTION_POINTER("Initialize", &Initialize_Method);
		CORECLR_GET_FUNCTION_POINTER("Load", &Load_Method);
		CORECLR_GET_FUNCTION_POINTER("RegisterSymbols", &RegisterSymbols_Method);
		CORECLR_GET_FUNCTION_POINTER("Unload", &Unload_Method);
		CORECLR_GET_FUNCTION_POINTER("UnregisterSymbols", &UnregisterSymbols_Method);

#undef CORECLR_GET_FUNCTION_POINTER

		check(FreeGCHandle_Method != nullptr);
		check(GetFunctionPointer_Method != nullptr);
		check(Initialize_Method != nullptr);
		check(Load_Method != nullptr);
		check(RegisterSymbols_Method != nullptr);
		check(Unload_Method != nullptr);
		check(UnregisterSymbols_Method != nullptr);

		FString ComponentAssemblyPath{ AssembliesDir / TEXT("NextTurn.UE.Programs" ".dll") };
		//FString ComponentAssemblyPath{ AssembliesDir / TEXT(PREPROCESSOR_TO_STRING(UE_PROJECT_NAME) ".dll") };
		auto ComponentAssemblyPathConv{ StringCast<UTF16CHAR>(*ComponentAssemblyPath) };
		Initialize_Method(
			reinterpret_cast<const char16_t*>(ComponentAssemblyPathConv.Get()),
			ComponentAssemblyPathConv.Length(),
			GetTypeExports(),
			GetTypeExportsLength());
	}

	void Load()
	{
		Load_Method();
	}

	void RegisterSymbols(const FTypeSymbol* Types, int32 TypesLength)
	{
		RegisterSymbols_Method(Types, TypesLength);
	}

	void Unload()
	{
		class FUnloadTask : public FNonAbandonableTask
		{
			friend FAutoDeleteAsyncTask<FUnloadTask>;

			void DoWork() const
			{
				int32 Retries = Unload_Method();
				RESTORE_WARNINGS;
				UE_LOG(LogNextTurnRuntime, Display, TEXT("Unloaded after %d garbage collections"), Retries + 1);
				ENABLE_WARNINGS;
			}

			TStatId GetStatId() const
			{
				RETURN_QUICK_DECLARE_CYCLE_STAT(FUnloadTask, STATGROUP_ThreadPoolAsyncTasks);
			}
		};
		
		(new FAutoDeleteAsyncTask<FUnloadTask>())->StartBackgroundTask();
	}

	void UnregisterSymbols(const FTypeSymbol* Types, int32 TypesLength)
	{
		UnregisterSymbols_Method(Types, TypesLength);
	}

	template<typename T>
	T GetFunctionPointer(
		const char16_t* AssemblyName,
		int32_t AssemblyNameLength,
		const char16_t* TypeName,
		int32_t TypeNameLength,
		const char16_t* MethodName,
		int32_t MethodNameLength)
	{
		return static_cast<T>(GetFunctionPointer_Method(
			AssemblyName,
			AssemblyNameLength,
			TypeName,
			TypeNameLength,
			MethodName,
			MethodNameLength));
	}
}

RESTORE_WARNINGS;
