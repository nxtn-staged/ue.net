// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Private/Accessor.hpp"
#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/CoreUObject/Public/UObject/Package.h"

ENABLE_WARNINGS;

DEFINE_OFFSET_ACCESSOR(UPackage, Guid);

#define TYPE_EXPORT_MANAGED_TYPE_NAME Package

TYPE_EXPORT_START

const FMemberSymbol Members[] =
{
	EXPORT_OFFSET_PUBLIC(UPackage, FileLength, FileSize),
	EXPORT_OFFSET_PUBLIC(UPackage, FileName, FileName),
	EXPORT_OFFSET(UPackage, Guid, Guid),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
