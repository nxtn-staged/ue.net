// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

#include "LefticeRuntime/Private/Exports.hpp"

#include "LefticeRuntime/Private/Accessor.hpp"
#include "LefticeRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/CoreUObject/Public/UObject/UnrealType.h"

ENABLE_WARNINGS;

DEFINE_OFFSET_ACCESSOR(UProperty, ArrayDim);
DEFINE_OFFSET_ACCESSOR(UProperty, ElementSize);
DEFINE_OFFSET_ACCESSOR(UProperty, Offset_Internal);
DEFINE_OFFSET_ACCESSOR(UProperty, PropertyFlags);

#define TYPE_EXPORT_MANAGED_TYPE_NAME Property

TYPE_EXPORT_START

void EXPORT_CALL_CONV ClearValue(const UProperty& Property, UObject* Object, int32_t index)
{
	Property.ClearValue_InContainer(Object, index);
}

void EXPORT_CALL_CONV FinalizeValues(const UProperty& Property, UObject* Object)
{
	Property.DestroyValue_InContainer(Object);
}

void* EXPORT_CALL_CONV GetValuePointer(const UProperty& Property, UObject* Object, int32_t Index)
{
	return Property.ContainerPtrToValuePtr<void>(Object, Index);
}

void EXPORT_CALL_CONV InitializeValues(const UProperty& Property, UObject* Object)
{
	Property.InitializeValue_InContainer(Object);
}

const FMemberSymbol Members[] =
{
	EXPORT_OFFSET(UProperty, ArrayLength, ArrayDim),
	EXPORT_OFFSET(UProperty, ElementSize, ElementSize),
	EXPORT_OFFSET(UProperty, Offset, Offset_Internal),
	EXPORT_OFFSET(UProperty, PropertyFlags, PropertyFlags),

	EXPORT_METHOD(ClearValue),
	EXPORT_METHOD(FinalizeValues),
	EXPORT_METHOD(GetValuePointer),
	EXPORT_METHOD(InitializeValues),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
