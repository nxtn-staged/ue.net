// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"
#include "Runtime/Core/Public/HAL/ConsoleManager.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME ConsoleObject

TYPE_EXPORT_START

const TCHAR* EXPORT_CALL_CONV GetHelpMessage(const IConsoleObject& Object)
{
	return Object.GetHelp();
}

void EXPORT_CALL_CONV SetHelpMessage(IConsoleObject& Object, const char16_t* HelpMessage)
{
	Object.SetHelp(CHAR16_TO_TCHAR(HelpMessage));
}

const FMemberSymbol Members[] =
{
	EXPORT_METHOD(GetHelpMessage),
	EXPORT_METHOD(SetHelpMessage),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
