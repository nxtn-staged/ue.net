// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Private/Accessor.hpp"
#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/CoreUObject/Public/UObject/Class.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME Method

TYPE_EXPORT_START

void EXPORT_CALL_CONV Invoke(UFunction& Method, UObject& Object, void* Parameters)
{
	Object.ProcessEvent(&Method, Parameters);
}

const FMemberSymbol Members[] =
{
	EXPORT_OFFSET_PUBLIC(UFunction, MethodFlags, FunctionFlags),
	EXPORT_OFFSET_PUBLIC(UFunction, ParameterCount, NumParms),
	EXPORT_OFFSET_PUBLIC(UFunction, ParametersSize, ParmsSize),
	EXPORT_OFFSET_PUBLIC(UFunction, ReturnValueOffset, ReturnValueOffset),

	EXPORT_METHOD(Invoke),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
