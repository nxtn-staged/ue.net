// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

#if WITH_APPLICATION_CORE

#include "LefticeRuntime/Private/Exports.hpp"

#include "LefticeRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/Slate/Public/Widgets/Layout/SBox.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME Box

TYPE_EXPORT_START

void EXPORT_CALL_CONV SetHorizontalAlignment(const TSharedRef<SBox>& Box, EHorizontalAlignment HorizontalAlignment)
{
	Box->SetHAlign(HorizontalAlignment);
}

void EXPORT_CALL_CONV SetVerticalAlignment(const TSharedRef<SBox>& Box, EVerticalAlignment VerticalAlignment)
{
	Box->SetVAlign(VerticalAlignment);
}

const FMemberSymbol Members[] =
{
	EXPORT_METHOD(SetHorizontalAlignment),
	EXPORT_METHOD(SetVerticalAlignment),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;

#endif
