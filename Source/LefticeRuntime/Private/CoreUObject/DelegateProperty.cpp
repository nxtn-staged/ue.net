// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

#include "LefticeRuntime/Private/Exports.hpp"

#include "LefticeRuntime/Private/Accessor.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/CoreUObject/Public/UObject/UnrealType.h"

DEFINE_OFFSET_ACCESSOR(UDelegateProperty, SignatureFunction);

#define TYPE_EXPORT_MANAGED_TYPE_NAME DelegateProperty

TYPE_EXPORT_START

const FMemberSymbol Members[] =
{
	EXPORT_OFFSET(UDelegateProperty, SignatureMethod, SignatureFunction),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME
