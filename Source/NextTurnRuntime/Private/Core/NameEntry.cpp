// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Private/Accessor.hpp"
#include "NextTurnRuntime/Public/Compiler.hpp"

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
