// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

ENABLE_WARNINGS;

RESTORE_WARNINGS;
DEFINE_LOG_CATEGORY_STATIC(LogNextTurn, VeryVerbose, All);
ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME Log

TYPE_EXPORT_START

void EXPORT_CALL_CONV Display(const char16_t* Message)
{
	RESTORE_WARNINGS;
	UE_LOG(LogNextTurn, Display, TEXT("%s"), CHAR16_TO_TCHAR(Message));
	ENABLE_WARNINGS;
}

void EXPORT_CALL_CONV Error(const char16_t* Message)
{
	RESTORE_WARNINGS;
	UE_LOG(LogNextTurn, Error, TEXT("%s"), CHAR16_TO_TCHAR(Message));
	ENABLE_WARNINGS;
}

void EXPORT_CALL_CONV Fatal(const char16_t* Message)
{
	RESTORE_WARNINGS;
	UE_LOG(LogNextTurn, Fatal, TEXT("%s"), CHAR16_TO_TCHAR(Message));
	ENABLE_WARNINGS;
}

void EXPORT_CALL_CONV Information(const char16_t* Message)
{
	RESTORE_WARNINGS;
	UE_LOG(LogNextTurn, Log, TEXT("%s"), CHAR16_TO_TCHAR(Message));
	ENABLE_WARNINGS;
}

void EXPORT_CALL_CONV Verbose(const char16_t* Message)
{
	RESTORE_WARNINGS;
	UE_LOG(LogNextTurn, Verbose, TEXT("%s"), CHAR16_TO_TCHAR(Message));
	ENABLE_WARNINGS;
}

void EXPORT_CALL_CONV VeryVerbose(const char16_t* Message)
{
	RESTORE_WARNINGS;
	UE_LOG(LogNextTurn, VeryVerbose, TEXT("%s"), CHAR16_TO_TCHAR(Message));
	ENABLE_WARNINGS;
}

void EXPORT_CALL_CONV Warning(const char16_t* Message)
{
	RESTORE_WARNINGS;
	UE_LOG(LogNextTurn, Warning, TEXT("%s"), CHAR16_TO_TCHAR(Message));
	ENABLE_WARNINGS;
}

const FMemberSymbol Members[] =
{
	EXPORT_METHOD(Display),
	EXPORT_METHOD(Error),
	EXPORT_METHOD(Fatal),
	EXPORT_METHOD(Information),
	EXPORT_METHOD(Verbose),
	EXPORT_METHOD(VeryVerbose),
	EXPORT_METHOD(Warning),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
