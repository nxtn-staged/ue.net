// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

#include "LefticeRuntime/Private/Exports.hpp"

#include "LefticeRuntime/Private/MarshaledTypes.hpp"
#include "LefticeRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"
#include "Runtime/Core/Public/HAL/ConsoleManager.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME ConsoleVariable

TYPE_EXPORT_START

int32_t EXPORT_CALL_CONV GetInt32(const IConsoleVariable& Variable)
{
	return Variable.GetInt();
}

float EXPORT_CALL_CONV GetSingle(const IConsoleVariable& Variable)
{
	return Variable.GetFloat();
}

void EXPORT_CALL_CONV GetString(const IConsoleVariable& Variable, FMarshaledString& Result)
{
	Result = Variable.GetString();
}

void EXPORT_CALL_CONV SetInt32(IConsoleVariable& Variable, int32_t Value)
{
	Variable.Set(Value);
}

void EXPORT_CALL_CONV SetSingle(IConsoleVariable& Variable, float Value)
{
	Variable.Set(Value);
}

void EXPORT_CALL_CONV SetString(IConsoleVariable& Variable, const char16_t* Value)
{
	Variable.Set(CHAR16_TO_TCHAR(Value));
}

const FMemberSymbol Members[] =
{
	EXPORT_METHOD(GetInt32),
	EXPORT_METHOD(GetSingle),
	EXPORT_METHOD(GetString),
	EXPORT_METHOD(SetInt32),
	EXPORT_METHOD(SetSingle),
	EXPORT_METHOD(SetString),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
