// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Private/Accessor.hpp"
#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/CoreUObject/Public/UObject/UnrealType.h"

ENABLE_WARNINGS;

DEFINE_OFFSET_ACCESSOR(FField, ClassPrivate);
DEFINE_OFFSET_ACCESSOR(FProperty, Offset_Internal);

#define TYPE_EXPORT_MANAGED_TYPE_NAME Property

TYPE_EXPORT_START

void EXPORT_CALL_CONV ClearValue(const FProperty& Property, UObject* Object, int32_t index)
{
	Property.ClearValue_InContainer(Object, index);
}

void EXPORT_CALL_CONV FinalizeValues(const FProperty& Property, UObject* Object)
{
	Property.DestroyValue_InContainer(Object);
}

void* EXPORT_CALL_CONV GetValuePointer(const FProperty& Property, UObject* Object, int32_t Index)
{
	return Property.ContainerPtrToValuePtr<void>(Object, Index);
}

void EXPORT_CALL_CONV InitializeValues(const FProperty& Property, UObject* Object)
{
	Property.InitializeValue_InContainer(Object);
}

const FMemberSymbol Members[] =
{
	EXPORT_OFFSET(FField, Class, ClassPrivate),
	EXPORT_OFFSET_PUBLIC(FField, Name, NamePrivate),
	EXPORT_OFFSET_PUBLIC(FField, Next, Next),
	EXPORT_OFFSET_PUBLIC(FProperty, ArrayLength, ArrayDim),
	EXPORT_OFFSET_PUBLIC(FProperty, ElementSize, ElementSize),
	EXPORT_OFFSET(FProperty, Offset, Offset_Internal),
	EXPORT_OFFSET_PUBLIC(FProperty, PropertyFlags, PropertyFlags),

	EXPORT_METHOD(ClearValue),
	EXPORT_METHOD(FinalizeValues),
	EXPORT_METHOD(GetValuePointer),
	EXPORT_METHOD(InitializeValues),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
