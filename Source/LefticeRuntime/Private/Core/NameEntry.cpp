// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

#include "LefticeRuntime/Private/Exports.hpp"

#include "LefticeRuntime/Private/Accessor.hpp"
#include "LefticeRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

ENABLE_WARNINGS;

DEFINE_OFFSET_ACCESSOR(FNameEntry, Header);
DEFINE_OFFSET_ACCESSOR(FNameEntry, AnsiName);

#define TYPE_EXPORT_MANAGED_TYPE_NAME NameEntry

TYPE_EXPORT_START

const FMemberSymbol Members[] =
{
	EXPORT_OFFSET(FNameEntry, Header, Header),
	EXPORT_OFFSET(FNameEntry, Name, AnsiName),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
