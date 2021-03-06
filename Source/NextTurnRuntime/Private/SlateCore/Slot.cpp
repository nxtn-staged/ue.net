// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#if WITH_APPLICATION_CORE

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/SlateCore/Public/SlotBase.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME Slot

TYPE_EXPORT_START

void EXPORT_CALL_CONV Attach(FSlotBase& Slot, const TSharedRef<SWidget>& Widget)
{
	Slot.AttachWidget(Widget);
}

const FMemberSymbol Members[] =
{
	EXPORT_METHOD(Attach),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;

#endif
