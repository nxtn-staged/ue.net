// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#if WITH_APPLICATION_CORE

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/Slate/Public/Framework/Commands/Commands.h"
#include "Runtime/Slate/Public/Framework/Commands/UICommandInfo.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME UICommandInfo

TYPE_EXPORT_START

void EXPORT_CALL_CONV Initialize(
	TSharedPtr<FUICommandInfo>& CommandInfo,
	const TSharedRef<FBindingContext>& Context,
	const char16_t* Name,
	const char16_t* UnderscoreTooltipName,
	const char16_t* DotName,
	const char16_t* FriendlyName,
	const char16_t* Description)
{
	MakeUICommand_InternalUseOnly(
		&Context.Get(),
		CommandInfo,
		TEXT("NextTurnRuntime"),
		CHAR16_TO_TCHAR(Name),
		CHAR16_TO_TCHAR(UnderscoreTooltipName),
		CHAR16_TO_ANSI(DotName),
		CHAR16_TO_TCHAR(FriendlyName),
		CHAR16_TO_TCHAR(Description),
		EUserInterfaceActionType::Button,
		FInputChord{});
}

void EXPORT_CALL_CONV Unregister(const TSharedRef<FBindingContext>& Context, const TSharedRef<FUICommandInfo>& CommandInfo)
{
	FUICommandInfo::UnregisterCommandInfo(Context, CommandInfo);
}

const FMemberSymbol Members[] =
{
	EXPORT_METHOD(Initialize),
	EXPORT_METHOD(Unregister),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;

#endif
