// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#if WITH_APPLICATION_CORE

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/SlateCore/Public/Widgets/SBoxPanel.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME HorizontalBoxSlot

TYPE_EXPORT_START

void EXPORT_CALL_CONV SetHorizontalAlignment(SHorizontalBox::FSlot& Slot, EHorizontalAlignment HorizontalAlignment)
{
	Slot.HAlign(HorizontalAlignment);
}

void EXPORT_CALL_CONV SetVerticalAlignment(SHorizontalBox::FSlot& Slot, EVerticalAlignment VerticalAlignment)
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
