// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Private/ManagedDelegate.hpp"
#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"
#include "Runtime/Core/Public/HAL/ConsoleManager.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME Console

TYPE_EXPORT_START

IConsoleCommand* EXPORT_CALL_CONV RegisterCommand(
	const char16_t* Name,
	const char16_t* HelpMessage,
	FConsoleCommandDelegate::FStaticDelegate::FFuncPtr Command,
	intptr_t CommandHandle)
{
	return IConsoleManager::Get().RegisterConsoleCommand(
		CHAR16_TO_TCHAR(Name),
		CHAR16_TO_TCHAR(HelpMessage),
		TManagedDelegate<FConsoleCommandDelegate>::Create(Command, CommandHandle));
}

IConsoleVariable* EXPORT_CALL_CONV RegisterInt32Variable(const char16_t* Name, const char16_t* HelpMessage, int32_t DefaultValue)
{
	return IConsoleManager::Get().RegisterConsoleVariable(CHAR16_TO_TCHAR(Name), DefaultValue, CHAR16_TO_TCHAR(HelpMessage));
}

IConsoleVariable* EXPORT_CALL_CONV RegisterSingleVariable(const char16_t* Name, const char16_t* HelpMessage, float DefaultValue)
{
	return IConsoleManager::Get().RegisterConsoleVariable(CHAR16_TO_TCHAR(Name), DefaultValue, CHAR16_TO_TCHAR(HelpMessage));
}

IConsoleVariable* EXPORT_CALL_CONV RegisterStringVariable(
	const char16_t* Name,
	const char16_t* HelpMessage,
	const char16_t* DefaultValue,
	int32_t DefaultValueLength)
{
	return IConsoleManager::Get().RegisterConsoleVariable(
		CHAR16_TO_TCHAR(Name),
		FString{ DefaultValueLength, CHAR16_TO_CHAR16(DefaultValue) },
		CHAR16_TO_TCHAR(HelpMessage));
}

void EXPORT_CALL_CONV Unregister(IConsoleObject* Object)
{
	IConsoleManager::Get().UnregisterConsoleObject(Object);
}

void EXPORT_CALL_CONV UnregisterWithName(const char16_t* Name)
{
	IConsoleManager::Get().UnregisterConsoleObject(CHAR16_TO_TCHAR(Name));
}

const FMemberSymbol Members[] =
{
	EXPORT_METHOD(RegisterCommand),
	EXPORT_METHOD(RegisterInt32Variable),
	EXPORT_METHOD(RegisterSingleVariable),
	EXPORT_METHOD(RegisterStringVariable),
	EXPORT_METHOD(Unregister),
	EXPORT_METHOD(UnregisterWithName),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

class _FConsole
{
	virtual auto EXPORT_CALL_CONV RegisterCommand(
		const char16_t* Name,
		const char16_t* HelpMessage,
		FConsoleCommandDelegate::FStaticDelegate::FFuncPtr Command,
		intptr_t CommandHandle) -> IConsoleCommand*;

	virtual auto EXPORT_CALL_CONV RegisterInt32Variable(
		const char16_t* Name,
		const char16_t* HelpMessage,
		int32_t DefaultValue) -> IConsoleVariable*;

	virtual auto EXPORT_CALL_CONV RegisterSingleVariable(
		const char16_t* Name,
		const char16_t* HelpMessage,
		float DefaultValue) -> IConsoleVariable*;

	virtual auto EXPORT_CALL_CONV RegisterStringVariable(
		const char16_t* Name,
		const char16_t* HelpMessage,
		const char16_t* DefaultValue,
		int32_t DefaultValueLength) -> IConsoleVariable*;

	virtual auto EXPORT_CALL_CONV Unregister(IConsoleObject* Object) -> void;

	virtual auto EXPORT_CALL_CONV UnregisterWithName(const char16_t* Name) -> void;
};

class _FConsole
{
	virtual auto EXPORT_CALL_CONV RegisterCommand(
		const char16_t* Name,
		const char16_t* HelpMessage,
		FConsoleCommandDelegate::FStaticDelegate::FFuncPtr Command,
		intptr_t CommandHandle) -> IConsoleCommand*
	{
		return IConsoleManager::Get().RegisterConsoleCommand(
			CHAR16_TO_TCHAR(Name),
			CHAR16_TO_TCHAR(HelpMessage),
			TManagedDelegate<FConsoleCommandDelegate>::Create(Command, CommandHandle));
	}

	virtual auto EXPORT_CALL_CONV RegisterInt32Variable(
		const char16_t* Name,
		const char16_t* HelpMessage,
		int32_t DefaultValue) -> IConsoleVariable*
	{
		return IConsoleManager::Get().RegisterConsoleVariable(CHAR16_TO_TCHAR(Name), DefaultValue, CHAR16_TO_TCHAR(HelpMessage));
	}

	virtual auto EXPORT_CALL_CONV RegisterSingleVariable(
		const char16_t* Name,
		const char16_t* HelpMessage,
		float DefaultValue) -> IConsoleVariable*
	{
		return IConsoleManager::Get().RegisterConsoleVariable(CHAR16_TO_TCHAR(Name), DefaultValue, CHAR16_TO_TCHAR(HelpMessage));
	}

	virtual auto EXPORT_CALL_CONV RegisterStringVariable(
		const char16_t* Name,
		const char16_t* HelpMessage,
		const char16_t* DefaultValue,
		int32_t DefaultValueLength) -> IConsoleVariable*
	{
		return IConsoleManager::Get().RegisterConsoleVariable(
			CHAR16_TO_TCHAR(Name),
			FString{ DefaultValueLength, CHAR16_TO_CHAR16(DefaultValue) },
			CHAR16_TO_TCHAR(HelpMessage));
	}

	virtual auto EXPORT_CALL_CONV Unregister(IConsoleObject* Object) -> void
	{
		IConsoleManager::Get().UnregisterConsoleObject(Object);
	}

	virtual auto EXPORT_CALL_CONV UnregisterWithName(const char16_t* Name) -> void
	{
		IConsoleManager::Get().UnregisterConsoleObject(CHAR16_TO_TCHAR(Name));
	}
};

RESTORE_WARNINGS;
