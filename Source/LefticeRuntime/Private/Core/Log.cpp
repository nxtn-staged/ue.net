// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

#include "LefticeRuntime/Private/Exports.hpp"

#include "LefticeRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

ENABLE_WARNINGS;

DEFINE_LOG_CATEGORY_STATIC(LogLeftice, VeryVerbose, All);

#define TYPE_EXPORT_MANAGED_TYPE_NAME Log

TYPE_EXPORT_START

void EXPORT_CALL_CONV Display(const char16_t* Message)
{
	RESTORE_WARNINGS;
	UE_LOG(LogLeftice, Display, TEXT("%s"), CHAR16_TO_TCHAR(Message));
	ENABLE_WARNINGS;
}

void EXPORT_CALL_CONV Error(const char16_t* Message)
{
	RESTORE_WARNINGS;
	UE_LOG(LogLeftice, Error, TEXT("%s"), CHAR16_TO_TCHAR(Message));
	ENABLE_WARNINGS;
}

void EXPORT_CALL_CONV Fatal(const char16_t* Message)
{
	RESTORE_WARNINGS;
	UE_LOG(LogLeftice, Fatal, TEXT("%s"), CHAR16_TO_TCHAR(Message));
	ENABLE_WARNINGS;
}

void EXPORT_CALL_CONV Information(const char16_t* Message)
{
	RESTORE_WARNINGS;
	UE_LOG(LogLeftice, Log, TEXT("%s"), CHAR16_TO_TCHAR(Message));
	ENABLE_WARNINGS;
}

void EXPORT_CALL_CONV Verbose(const char16_t* Message)
{
	RESTORE_WARNINGS;
	UE_LOG(LogLeftice, Verbose, TEXT("%s"), CHAR16_TO_TCHAR(Message));
	ENABLE_WARNINGS;
}

void EXPORT_CALL_CONV VeryVerbose(const char16_t* Message)
{
	RESTORE_WARNINGS;
	UE_LOG(LogLeftice, VeryVerbose, TEXT("%s"), CHAR16_TO_TCHAR(Message));
	ENABLE_WARNINGS;
}

void EXPORT_CALL_CONV Warning(const char16_t* Message)
{
	RESTORE_WARNINGS;
	UE_LOG(LogLeftice, Warning, TEXT("%s"), CHAR16_TO_TCHAR(Message));
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
