// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

#include "LefticeRuntime/Private/Exports.hpp"

#include "LefticeRuntime/Private/Accessor.hpp"
#include "LefticeRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/CoreUObject/Public/UObject/UnrealType.h"

ENABLE_WARNINGS;

DEFINE_OFFSET_ACCESSOR(UMapProperty, KeyProp);
DEFINE_OFFSET_ACCESSOR(UMapProperty, ValueProp);
DEFINE_OFFSET_ACCESSOR(UMapProperty, MapLayout);

#define TYPE_EXPORT_MANAGED_TYPE_NAME MapProperty

TYPE_EXPORT_START

const FMemberSymbol Members[] =
{
	EXPORT_OFFSET(UMapProperty, KeyProperty, KeyProp),
	EXPORT_OFFSET(UMapProperty, ValueProperty, ValueProp),
	EXPORT_OFFSET(UMapProperty, Layout, MapLayout),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
