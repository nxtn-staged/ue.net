// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Private/Accessor.hpp"
#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/CoreUObject/Public/UObject/UnrealType.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME MulticastDelegateProperty

TYPE_EXPORT_START

const FMulticastScriptDelegate* EXPORT_CALL_CONV GetValuePointer(
	const FMulticastDelegateProperty& Property, const UObject* Object, int32_t Index)
{
	return Property.GetMulticastDelegate(Property.ContainerPtrToValuePtr<void>(Object, Index));
}

void EXPORT_CALL_CONV SetValue(
	const FMulticastDelegateProperty& Property, UObject* Object, int32_t Index, const FMulticastScriptDelegate& Value)
{
	Property.SetMulticastDelegate(Property.ContainerPtrToValuePtr<void>(Object, Index), Value);
}

const FMemberSymbol Members[] =
{
	EXPORT_OFFSET_PUBLIC(FMulticastDelegateProperty, SignatureMethod, SignatureFunction),

	EXPORT_METHOD(GetValuePointer),
	EXPORT_METHOD(SetValue),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
