// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Private/Accessor.hpp"
#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

ENABLE_WARNINGS;

using FDefaultScriptArray = TScriptArray<FHeapAllocator>;
DEFINE_METHOD_ACCESSOR(FDefaultScriptArray, ResizeGrow, void, int32, int32);

#define TYPE_EXPORT_MANAGED_TYPE_NAME ScriptArray

TYPE_EXPORT_START

void EXPORT_CALL_CONV Finalize(FScriptArray& Array)
{
	Array.~FScriptArray();
}

void EXPORT_CALL_CONV ResizeGrow(FScriptArray& Array, int32_t OldCount, int32_t ItemSize)
{
	INVOKE(FDefaultScriptArray, ResizeGrow, Array, OldCount, ItemSize);
}

const FMemberSymbol Members[] =
{
	EXPORT_METHOD(Finalize),
	EXPORT_METHOD(ResizeGrow),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
