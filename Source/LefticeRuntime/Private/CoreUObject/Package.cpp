// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

#include "LefticeRuntime/Private/Exports.hpp"

#include "LefticeRuntime/Private/Accessor.hpp"
#include "LefticeRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/CoreUObject/Public/UObject/Package.h"

ENABLE_WARNINGS;

DEFINE_OFFSET_ACCESSOR(UPackage, FileSize);
DEFINE_OFFSET_ACCESSOR(UPackage, FileName);
DEFINE_OFFSET_ACCESSOR(UPackage, Guid);

#define TYPE_EXPORT_MANAGED_TYPE_NAME Package

TYPE_EXPORT_START

const FMemberSymbol Members[] =
{
	EXPORT_OFFSET(UPackage, FileLength, FileSize),
	EXPORT_OFFSET(UPackage, FileName, FileName),
	EXPORT_OFFSET(UPackage, Guid, Guid),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
