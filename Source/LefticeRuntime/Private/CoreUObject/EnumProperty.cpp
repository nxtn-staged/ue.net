// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

#include "LefticeRuntime/Private/Exports.hpp"

#include "LefticeRuntime/Private/Accessor.hpp"
#include "LefticeRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/CoreUObject/Public/UObject/EnumProperty.h"

ENABLE_WARNINGS;

DEFINE_OFFSET_ACCESSOR(UEnumProperty, Enum);
DEFINE_OFFSET_ACCESSOR(UEnumProperty, UnderlyingProp);

#define TYPE_EXPORT_MANAGED_TYPE_NAME EnumProperty

TYPE_EXPORT_START

const FMemberSymbol Members[] =
{
	EXPORT_OFFSET(UEnumProperty, MetaEnum, Enum),
	EXPORT_OFFSET(UEnumProperty, UnderlyingProperty, UnderlyingProp),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
