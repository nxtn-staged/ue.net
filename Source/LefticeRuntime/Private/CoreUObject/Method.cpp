// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

#include "LefticeRuntime/Private/Exports.hpp"

#include "LefticeRuntime/Private/Accessor.hpp"
#include "LefticeRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/CoreUObject/Public/UObject/Class.h"

ENABLE_WARNINGS;

DEFINE_OFFSET_ACCESSOR(UFunction, FunctionFlags);
DEFINE_OFFSET_ACCESSOR(UFunction, NumParms);
DEFINE_OFFSET_ACCESSOR(UFunction, ParmsSize);
DEFINE_OFFSET_ACCESSOR(UFunction, ReturnValueOffset);

#define TYPE_EXPORT_MANAGED_TYPE_NAME Method

TYPE_EXPORT_START

const FMemberSymbol Members[] =
{
	EXPORT_OFFSET(UFunction, MethodFlags, FunctionFlags),
	EXPORT_OFFSET(UFunction, ParameterCount, NumParms),
	EXPORT_OFFSET(UFunction, ParameterSize, ParmsSize),
	EXPORT_OFFSET(UFunction, ReturnValueOffset, ReturnValueOffset),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
