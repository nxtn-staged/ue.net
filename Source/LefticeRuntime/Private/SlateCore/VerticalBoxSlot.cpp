// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

#if WITH_APPLICATION_CORE

#include "LefticeRuntime/Private/Exports.hpp"

#include "LefticeRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/SlateCore/Public/Widgets/SBoxPanel.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME VerticalBoxSlot

TYPE_EXPORT_START

void EXPORT_CALL_CONV SetHorizontalAlignment(SVerticalBox::FSlot& Slot, EHorizontalAlignment HorizontalAlignment)
{
	Slot.HAlign(HorizontalAlignment);
}

void EXPORT_CALL_CONV SetVerticalAlignment(SVerticalBox::FSlot& Slot, EVerticalAlignment VerticalAlignment)
{
	Slot.VAlign(VerticalAlignment);
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
