// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#pragma once

#include "NextTurnRuntime/Private/Exports.hpp"
#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

ENABLE_WARNINGS;

namespace CoreClr
{
	inline void FreeGCHandle(intptr_t Handle);
	void Initialize();
	inline void Load();
	inline void RegisterSymbols(const FTypeSymbol* Types, int32 TypesLength);
	void Unload();
	inline void UnregisterSymbols(const FTypeSymbol* Types, int32 TypesLength);

	template<typename T>
	inline T GetFunctionPointer(
		const char16_t* AssemblyName,
		int32_t AssemblyNameLength,
		const char16_t* TypeName,
		int32_t TypeNameLength,
		const char16_t* MethodName,
		int32_t MethodNameLength);
}

RESTORE_WARNINGS;
