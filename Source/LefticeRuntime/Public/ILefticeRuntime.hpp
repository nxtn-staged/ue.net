// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

#pragma once

#include "LefticeRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"
#include "Runtime/Core/Public/Modules/ModuleManager.h"

ENABLE_WARNINGS;

#define GET_FUNCTION_POINTER(T, AssemblyName, TypeName, MethodName) \
GetFunctionPointer<T>(AssemblyName, TypeName, MethodName, MethodName u"_Delegate");

class LEFTICERUNTIME_API ILefticeRuntime : public IModuleInterface
{
public:
	static const FName LefticeRuntimeModuleName;

	static ILefticeRuntime& Get()
	{
		return FModuleManager::LoadModuleChecked<ILefticeRuntime>(LefticeRuntimeModuleName);
	}

	static bool IsAvailable()
	{
		return FModuleManager::Get().IsModuleLoaded(LefticeRuntimeModuleName);
	}

	virtual void* GetFunctionPointer(
		const char16_t* AssemblyName,
		int32_t AssemblyNameLength,
		const char16_t* TypeName,
		int32_t TypeNameLength,
		const char16_t* MethodName,
		int32_t MethodNameLength,
		const char16_t* DelegateTypeName,
		int32_t DelegateTypeNameLength) const = 0;

	template<typename T, int32_t AssemblyNameSize, int32_t TypeNameSize, int32_t MethodNameSize, int32_t DelegateTypeNameSize>
	T GetFunctionPointer(
		const char16_t (&AssemblyName)[AssemblyNameSize],
		const char16_t (&TypeName)[TypeNameSize],
		const char16_t (&MethodName)[MethodNameSize],
		const char16_t (&DelegateTypeName)[DelegateTypeNameSize])
	{
		return reinterpret_cast<T>(this->GetFunctionPointer(
			AssemblyName,
			AssemblyNameSize - 1,
			TypeName,
			TypeNameSize - 1,
			MethodName,
			MethodNameSize - 1,
			DelegateTypeName,
			DelegateTypeNameSize - 1));
	}
};

RESTORE_WARNINGS;
