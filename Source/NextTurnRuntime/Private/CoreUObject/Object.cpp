// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Private/Accessor.hpp"
#include "NextTurnRuntime/Private/MarshaledTypes.hpp"
#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/CoreUObject/Public/UObject/Class.h"

ENABLE_WARNINGS;

DEFINE_OFFSET_ACCESSOR(UObjectBase, ClassPrivate);
DEFINE_OFFSET_ACCESSOR(UObjectBase, NamePrivate);
DEFINE_OFFSET_ACCESSOR(UObjectBase, OuterPrivate);

#define TYPE_EXPORT_MANAGED_TYPE_NAME Object

TYPE_EXPORT_START

UObject* EXPORT_CALL_CONV Create(UClass* Class)
{
	return StaticConstructObject_Internal(Class);
}

UPackage* EXPORT_CALL_CONV GetPackage(const UObject& Object)
{
	return Object.GetOutermost();
}

void EXPORT_CALL_CONV GetPathName(const UObject& Object, FMarshaledString& Result)
{
	Result = Object.GetPathName();
}

const FMemberSymbol Members[] =
{
	EXPORT_OFFSET(UObjectBase, Class, ClassPrivate),
	EXPORT_OFFSET(UObjectBase, Name, NamePrivate),
	EXPORT_OFFSET(UObjectBase, Parent, OuterPrivate),

	EXPORT_METHOD(Create),
	EXPORT_METHOD(GetPackage),
	EXPORT_METHOD(GetPathName),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
