// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#if WITH_APPLICATION_CORE

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/Slate/Public/Framework/MultiBox/MultiBoxBuilder.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME MenuBuilder

TYPE_EXPORT_START

void EXPORT_CALL_CONV AddMenuEntry(FMenuBuilder& Builder, const TSharedRef<const FUICommandInfo>& CommandInfo)
{
	Builder.AddMenuEntry(CommandInfo);
}

void EXPORT_CALL_CONV AddSeparator(FMenuBuilder& Builder, FName ExtensionPoint)
{
	Builder.AddMenuSeparator(ExtensionPoint);
}

void EXPORT_CALL_CONV BeginSection(FMenuBuilder& Builder, FName ExtensionPoint)
{
	Builder.BeginSection(ExtensionPoint);
}

void EXPORT_CALL_CONV EndSection(FMenuBuilder& Builder)
{
	Builder.EndSection();
}

const FMemberSymbol Members[] =
{
	EXPORT_METHOD(AddMenuEntry),
	EXPORT_METHOD(AddSeparator),
	EXPORT_METHOD(BeginSection),
	EXPORT_METHOD(EndSection),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;

#endif
