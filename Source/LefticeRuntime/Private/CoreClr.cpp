// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

#include "CoreClr.hpp"

#include "Exports.hpp"
#include "LefticeRuntime.hpp"

#include <nethost.h>

#include <hostfxr.h>
#include <coreclr_delegates.h>

#include "Runtime/Core/Public/HAL/PlatformProcess.h"
#include "Runtime/Core/Public/Misc/Paths.h"

ENABLE_WARNINGS;

namespace Assert
{
	template<SIZE_T N, typename... Types>
	void Fail(const TCHAR (&Format)[N], Types... Arguments)
	{
		RESTORE_WARNINGS;
		UE_LOG(LogLefticeRuntime, Fatal, Format, Arguments...);
		ENABLE_WARNINGS;
	}

	template<SIZE_T N, typename... Types>
	void NotNull(void* Ptr, const TCHAR (&Format)[N], Types... Arguments)
	{
		if (Ptr == nullptr)
		{
			Fail(Format, Arguments...);
		}
	}

	template<SIZE_T N, typename... Types>
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
	void* (__stdcall *GetFunctionPointer_Method)(
		const char16_t* AssemblyName,
		int32_t AssemblyNameLength,
		const char16_t* TypeName,
		int32_t TypeNameLength,
		const char16_t* MethodName,
		int32_t MethodNameLength,
		const char16_t* DelegateTypeName,
		int32_t DelegateTypeNameLength);

	void (__stdcall *Load_Method)();
	int32_t (__stdcall *Unload_Method)();

	void Initialize()
	{
		FString AssembliesDir{ FPaths::ProjectDir() / TEXT("Binaries") / TEXT("UE.NET") };

		TArray<char_t> PathBuffer{};
		int32 PathBufferSize = FPlatformMisc::GetMaxPathLength();
		PathBuffer.AddUninitialized(PathBufferSize);
		size_t PathBufferSizeSize = static_cast<size_t>(PathBufferSize);

		auto AssembliesDirConv{ Pal::TCharToPalConv(*AssembliesDir) };
		get_hostfxr_parameters Parameters{ sizeof(get_hostfxr_parameters), AssembliesDirConv.Get() };

		int32 Status = get_hostfxr_path(PathBuffer.GetData(), &PathBufferSizeSize, &Parameters);
		Assert::Succeeded(Status, TEXT("Failed to locate Host FXR: 0x%x"), Status);

		// Leak Host FXR for module reloading
		void* HostFxrLib = FPlatformProcess::GetDllHandle(PathBuffer.GetData());
		Assert::NotNull(HostFxrLib, TEXT("Failed to load Host FXR"));

		auto HostFxrSetErrorWriter = static_cast<hostfxr_set_error_writer_fn>(
			FPlatformProcess::GetDllExport(HostFxrLib, TEXT("hostfxr_set_error_writer")));

		auto HostFxrInitialize = static_cast<hostfxr_initialize_for_runtime_config_fn>(
			FPlatformProcess::GetDllExport(HostFxrLib, TEXT("hostfxr_initialize_for_runtime_config")));

		auto HostFxrGetDelegate = static_cast<hostfxr_get_runtime_delegate_fn>(
			FPlatformProcess::GetDllExport(HostFxrLib, TEXT("hostfxr_get_runtime_delegate")));

		auto HostFxrClose = static_cast<hostfxr_close_fn>(
			FPlatformProcess::GetDllExport(HostFxrLib, TEXT("hostfxr_close")));

		check(HostFxrSetErrorWriter != nullptr);
		check(HostFxrInitialize != nullptr);
		check(HostFxrGetDelegate != nullptr);
		check(HostFxrClose != nullptr);

		HostFxrSetErrorWriter(
			[](const char_t* Message)
			{
				RESTORE_WARNINGS;
				UE_LOG(LogLefticeRuntime, Error, TEXT("%s"), Pal::PalToTCharConv(Message).Get());
				ENABLE_WARNINGS;
			});

		hostfxr_handle ContextHandle;

		Status = HostFxrInitialize(
			Pal::TCharToPalConv(*(AssembliesDir / TEXT("Leftice.Loader.runtimeconfig.json"))).Get(),
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

#define LEFTICE_LOADER "Leftice.Loader"
#define LEFTICE_LOADER_MAKE_DELEGATE_TYPE_NAME(MethodName) LEFTICE_LOADER "+" MethodName "_Delegate, " LEFTICE_LOADER

		FString LoaderAssemblyPath{ AssembliesDir / TEXT(LEFTICE_LOADER ".dll") };
		auto LoaderAssemblyPathConv{ Pal::TCharToPalConv(*LoaderAssemblyPath) };

		const char_t* LoaderTypeName = PALTEXT(LEFTICE_LOADER ", " LEFTICE_LOADER);

		CoreClrGetFunctionPointer(
			LoaderAssemblyPathConv.Get(),
			LoaderTypeName,
			PALTEXT("GetFunctionPointer"),
			PALTEXT(LEFTICE_LOADER_MAKE_DELEGATE_TYPE_NAME("GetFunctionPointer")),
			nullptr,
			reinterpret_cast<void**>(&GetFunctionPointer_Method));

		void (__stdcall *Initialize_Method)(
			const char16_t* ComponentAssemblyPath,
			int32 ComponentAssemblyPathLength,
			const FTypeSymbol* Types,
			int32 TypesLength);

		CoreClrGetFunctionPointer(
			LoaderAssemblyPathConv.Get(),
			LoaderTypeName,
			PALTEXT("Initialize"),
			PALTEXT(LEFTICE_LOADER_MAKE_DELEGATE_TYPE_NAME("Initialize")),
			nullptr,
			reinterpret_cast<void**>(&Initialize_Method));

		CoreClrGetFunctionPointer(
			LoaderAssemblyPathConv.Get(),
			LoaderTypeName,
			PALTEXT("Load"),
			PALTEXT(LEFTICE_LOADER_MAKE_DELEGATE_TYPE_NAME("Load")),
			nullptr,
			reinterpret_cast<void**>(&Load_Method));

		CoreClrGetFunctionPointer(
			LoaderAssemblyPathConv.Get(),
			LoaderTypeName,
			PALTEXT("Unload"),
			PALTEXT(LEFTICE_LOADER_MAKE_DELEGATE_TYPE_NAME("Unload")),
			nullptr,
			reinterpret_cast<void**>(&Unload_Method));

		check(GetFunctionPointer_Method != nullptr);
		check(Initialize_Method != nullptr);
		check(Load_Method != nullptr);
		check(Unload_Method != nullptr);

		FString ComponentAssemblyPath{ AssembliesDir / TEXT("Leftice.Editor" ".dll") };
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

	void Unload()
	{
		int32 Retries = Unload_Method();
		RESTORE_WARNINGS;
		UE_LOG(LogLefticeRuntime, Display, TEXT("Unloaded after %d garbage collections"), Retries + 1);
		ENABLE_WARNINGS;
	}

	template<typename T>
	T GetFunctionPointer(
		const char16_t* AssemblyName,
		int32_t AssemblyNameLength,
		const char16_t* TypeName,
		int32_t TypeNameLength,
		const char16_t* MethodName,
		int32_t MethodNameLength,
		const char16_t* DelegateTypeName,
		int32_t DelegateTypeNameLength)
	{
		return static_cast<T>(GetFunctionPointer_Method(
			AssemblyName,
			AssemblyNameLength,
			TypeName,
			TypeNameLength,
			MethodName,
			MethodNameLength,
			DelegateTypeName,
			DelegateTypeNameLength));
	}
}

RESTORE_WARNINGS;
