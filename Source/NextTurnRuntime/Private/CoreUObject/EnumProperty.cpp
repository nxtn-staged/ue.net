// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Private/Accessor.hpp"
#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/CoreUObject/Public/UObject/EnumProperty.h"

ENABLE_WARNINGS;

DEFINE_OFFSET_ACCESSOR(FEnumProperty, Enum);
DEFINE_OFFSET_ACCESSOR(FEnumProperty, UnderlyingProp);

#define TYPE_EXPORT_MANAGED_TYPE_NAME EnumProperty

TYPE_EXPORT_START

const FMemberSymbol Members[] =
{
	EXPORT_OFFSET(FEnumProperty, MetaEnum, Enum),
	EXPORT_OFFSET(FEnumProperty, UnderlyingProperty, UnderlyingProp),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
