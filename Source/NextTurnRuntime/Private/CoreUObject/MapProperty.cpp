// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Private/Accessor.hpp"
#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/CoreUObject/Public/UObject/UnrealType.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME MapProperty

TYPE_EXPORT_START

const FMemberSymbol Members[] =
{
	EXPORT_OFFSET_PUBLIC(FMapProperty, KeyProperty, KeyProp),
	EXPORT_OFFSET_PUBLIC(FMapProperty, ValueProperty, ValueProp),
	EXPORT_OFFSET_PUBLIC(FMapProperty, Layout, MapLayout),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
