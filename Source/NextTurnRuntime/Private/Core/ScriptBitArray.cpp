// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Private/Accessor.hpp"
#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

ENABLE_WARNINGS;

//void ScriptArray_Finalize(FScriptArray& Array)
//{
//	Array.~FScriptArray();
//}

using FDefaultScriptBitArray = TScriptBitArray<FDefaultBitArrayAllocator, FScriptBitArray>;
DEFINE_METHOD_ACCESSOR(FDefaultScriptBitArray, Realloc, void, int32);
DEFINE_METHOD_ACCESSOR(FDefaultScriptBitArray, ReallocGrow, void, int32);

#define TYPE_EXPORT_MANAGED_TYPE_NAME ScriptBitArray

TYPE_EXPORT_START

void EXPORT_CALL_CONV Resize(FScriptBitArray& Array, int32_t OldCount)
{
	INVOKE(FDefaultScriptBitArray, Realloc, Array, OldCount);
}

void EXPORT_CALL_CONV ResizeGrow(FScriptBitArray& Array, int32_t OldCount)
{
	INVOKE(FDefaultScriptBitArray, ReallocGrow, Array, OldCount);
}

const FMemberSymbol Members[] =
{
	EXPORT_METHOD(Resize),
	EXPORT_METHOD(ResizeGrow),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
