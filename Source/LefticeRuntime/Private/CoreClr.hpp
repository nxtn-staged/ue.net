// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

#pragma once

#include "LefticeRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

ENABLE_WARNINGS;

namespace CoreClr
{
	void Initialize();
	inline void Load();
	void Unload();

	template<typename T>
	inline T GetFunctionPointer(
		const char16_t* AssemblyName,
		int32_t AssemblyNameLength,
		const char16_t* TypeName,
		int32_t TypeNameLength,
		const char16_t* MethodName,
		int32_t MethodNameLength,
		const char16_t* DelegateTypeName,
		int32_t DelegateTypeNameLength);
}

RESTORE_WARNINGS;
