// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#pragma once

#include "NextTurnRuntime/Private/Exports.hpp"
#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"
#include "Runtime/Core/Public/Modules/ModuleManager.h"

ENABLE_WARNINGS;

class NEXTTURNRUNTIME_API INextTurnRuntime : public IModuleInterface
{
public:
	static const FName NextTurnRuntimeModuleName;

	static INextTurnRuntime& Get()
	{
		return FModuleManager::LoadModuleChecked<INextTurnRuntime>(NextTurnRuntimeModuleName);
	}

	static bool IsAvailable()
	{
		return FModuleManager::Get().IsModuleLoaded(NextTurnRuntimeModuleName);
	}

	virtual void* GetFunctionPointer(
		const char16_t* AssemblyName,
		int32_t AssemblyNameLength,
		const char16_t* TypeName,
		int32_t TypeNameLength,
		const char16_t* MethodName,
		int32_t MethodNameLength) const = 0;

	virtual void RegisterSymbols(const FTypeSymbol* Types, int32 TypesLength) const = 0;
	virtual void UnregisterSymbols(const FTypeSymbol* Types, int32 TypesLength) const = 0;

	template<typename T, int32_t AssemblyNameSize, int32_t TypeNameSize, int32_t MethodNameSize>
	T GetFunctionPointer(
		const char16_t (&AssemblyName)[AssemblyNameSize],
		const char16_t (&TypeName)[TypeNameSize],
		const char16_t (&MethodName)[MethodNameSize])
	{
		return reinterpret_cast<T>(this->GetFunctionPointer(
			AssemblyName,
			AssemblyNameSize - 1,
			TypeName,
			TypeNameSize - 1,
			MethodName,
			MethodNameSize - 1));
	}
};

RESTORE_WARNINGS;
