// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

#include "LefticeRuntime/Private/Exports.hpp"

#include "LefticeRuntime/Private/Accessor.hpp"
#include "LefticeRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/CoreUObject/Public/UObject/UnrealType.h"

ENABLE_WARNINGS;

DEFINE_OFFSET_ACCESSOR(UClassProperty, MetaClass);

#define TYPE_EXPORT_MANAGED_TYPE_NAME ClassProperty

TYPE_EXPORT_START

const FMemberSymbol Members[] =
{
	EXPORT_OFFSET(UClassProperty, MetaClass, MetaClass),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
