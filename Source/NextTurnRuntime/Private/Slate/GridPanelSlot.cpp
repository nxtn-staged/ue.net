// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#if WITH_APPLICATION_CORE

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/Slate/Public/Widgets/Layout/SGridPanel.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME GridPanelSlot

TYPE_EXPORT_START

void EXPORT_CALL_CONV SetColumn(SGridPanel::FSlot& Slot, int32_t Column)
{
	Slot.Column(Column);
}

void EXPORT_CALL_CONV SetColumnSpan(SGridPanel::FSlot& Slot, int32_t ColumnSpan)
{
	Slot.ColumnSpan(ColumnSpan);
}

void EXPORT_CALL_CONV SetRow(SGridPanel::FSlot& Slot, int32_t Row)
{
	Slot.Row(Row);
}

void EXPORT_CALL_CONV SetRowSpan(SGridPanel::FSlot& Slot, int32_t RowSpan)
{
	Slot.RowSpan(RowSpan);
}

const FMemberSymbol Members[] =
{
	EXPORT_METHOD(SetColumn),
	EXPORT_METHOD(SetColumnSpan),
	EXPORT_METHOD(SetRow),
	EXPORT_METHOD(SetRowSpan),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;

#endif
