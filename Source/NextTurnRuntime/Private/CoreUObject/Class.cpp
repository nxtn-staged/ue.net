// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Private/Accessor.hpp"
#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/CoreUObject/Public/UObject/Class.h"
#include "Runtime/CoreUObject/Public/UObject/UObjectHash.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME Class

TYPE_EXPORT_START

UFunction* EXPORT_CALL_CONV FindMethod(const UClass& Class, FName Name)
{
	return Class.FindFunctionByName(Name);
}

UObject* FindObject(UClass* Class, const char16_t* Name)
{
	return StaticFindObject(Class, ANY_PACKAGE, CHAR16_TO_TCHAR(Name));
}

void FindObjects(UClass* Class, TArray<UObject*>& Result)
{
	GetObjectsOfClass(Class, Result);
}

void FindSubclasses(UClass* Class, TArray<UClass*>& Result)
{
	GetDerivedClasses(Class, Result);
}

const FMemberSymbol Members[] =
{
	EXPORT_OFFSET_PUBLIC(UClass, ClassFlags, ClassFlags),
	EXPORT_OFFSET_PUBLIC(UClass, ClassTypeFlags, ClassCastFlags),
	EXPORT_OFFSET_PUBLIC(UClass, Interfaces, Interfaces),

	EXPORT_METHOD(FindMethod),
	EXPORT_METHOD(FindObject),
	EXPORT_METHOD(FindObjects),
	EXPORT_METHOD(FindSubclasses),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
