// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/CoreUObject/Public/UObject/UnrealType.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME BooleanProperty

TYPE_EXPORT_START

bool EXPORT_CALL_CONV GetValue(const FBoolProperty& Property, const UObject* Object, int32_t Index)
{
	return Property.GetPropertyValue_InContainer(Object, Index);
}

void EXPORT_CALL_CONV SetValue(const FBoolProperty& Property, UObject* Object, bool Value, int32_t Index)
{
	Property.SetPropertyValue_InContainer(Object, Value, Index);
}

const FMemberSymbol Members[] =
{
	EXPORT_METHOD(GetValue),
	EXPORT_METHOD(SetValue),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
