// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME Memory

TYPE_EXPORT_START

void* EXPORT_CALL_CONV Alloc(int32_t Size)
{
	return FMemory::Malloc(static_cast<SIZE_T>(Size));
}

void EXPORT_CALL_CONV Free(void* Ptr)
{
	FMemory::Free(Ptr);
}

const FMemberSymbol Members[] =
{
	EXPORT_METHOD(Alloc),
	EXPORT_METHOD(Free),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
