// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

#include "LefticeRuntime/Private/Exports.hpp"

#include "LefticeRuntime/Private/Accessor.hpp"
#include "LefticeRuntime/Private/Construct.hpp"
#include "LefticeRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/CoreUObject/Public/UObject/WeakObjectPtr.h"

ENABLE_WARNINGS;

struct FLefticeWeakObjectReference
{
	int32_t Index;
	int32_t SerialNumber;
};

DEFINE_SIZE_ACCESSOR(FWeakObjectPtr, ObjectIndex);
DEFINE_SIZE_ACCESSOR(FWeakObjectPtr, ObjectSerialNumber);

static_assert(sizeof(FLefticeWeakObjectReference) == sizeof(FWeakObjectPtr), "");

static_assert(sizeof(FLefticeWeakObjectReference::Index) == SIZEOF(FWeakObjectPtr, ObjectIndex), "");
static_assert(sizeof(FLefticeWeakObjectReference::SerialNumber) == SIZEOF(FWeakObjectPtr, ObjectSerialNumber), "");

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
