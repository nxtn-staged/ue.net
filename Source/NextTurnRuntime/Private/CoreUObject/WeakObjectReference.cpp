// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Private/Accessor.hpp"
#include "NextTurnRuntime/Private/Construct.hpp"
#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/CoreUObject/Public/UObject/WeakObjectPtr.h"

ENABLE_WARNINGS;

struct FNextTurnWeakObjectReference
{
	int32_t Index;
	int32_t SerialNumber;
};

DEFINE_SIZE_ACCESSOR(FWeakObjectPtr, ObjectIndex);
DEFINE_SIZE_ACCESSOR(FWeakObjectPtr, ObjectSerialNumber);

static_assert(sizeof(FNextTurnWeakObjectReference) == sizeof(FWeakObjectPtr), "");

static_assert(sizeof(FNextTurnWeakObjectReference::Index) == SIZEOF(FWeakObjectPtr, ObjectIndex), "");
static_assert(sizeof(FNextTurnWeakObjectReference::SerialNumber) == SIZEOF(FWeakObjectPtr, ObjectSerialNumber), "");

#define TYPE_EXPORT_MANAGED_TYPE_NAME WeakObjectReference

TYPE_EXPORT_START

void EXPORT_CALL_CONV Initialize(FWeakObjectPtr& Reference, const UObject* Target)
{
	Construct(Reference, Target);
}

UObject* EXPORT_CALL_CONV GetTarget(const FWeakObjectPtr& Reference)
{
	return Reference.Get();
}

bool EXPORT_CALL_CONV IsValid(const FWeakObjectPtr& Reference)
{
	return Reference.IsValid();
}

const FMemberSymbol Members[] =
{
	EXPORT_METHOD(Initialize),
	EXPORT_METHOD(GetTarget),
	EXPORT_METHOD(IsValid),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
