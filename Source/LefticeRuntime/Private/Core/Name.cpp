// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

#include "LefticeRuntime/Private/Exports.hpp"

#include "LefticeRuntime/Private/MarshaledTypes.hpp"
#include "LefticeRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME Name

TYPE_EXPORT_START

void EXPORT_CALL_CONV InitializeAnsi(FMarshaledName& Name, int32_t Length, const char16_t* Str, int32_t Number, EFindName Mode)
{
	Name = FName{ Length, CHAR16_TO_ANSI(Str), Number, Mode };
}

void EXPORT_CALL_CONV InitializeWide(FMarshaledName& Name, int32_t Length, const char16_t* Str, int32_t Number, EFindName Mode)
{
	Name = FName{ Length, CHAR16_TO_WIDE(Str), Number, Mode };
}

int32_t EXPORT_CALL_CONV CompareTo(const FMarshaledName Name, const FMarshaledName Other)
{
	return Name.ToName().Compare(Other.ToName());
}

const FNameEntry* EXPORT_CALL_CONV GetDisplayNameEntry(const FMarshaledName Name)
{
	return Name.ToName().GetDisplayNameEntry();
}

int32_t EXPORT_CALL_CONV GetHashCode(const FMarshaledName Name)
{
	return static_cast<int32_t>(GetTypeHash(Name.ToName()));
}

bool EXPORT_CALL_CONV IsValid(const FMarshaledName Name)
{
	return Name.ToName().IsValid();
}

const FMemberSymbol Members[] =
{
	EXPORT_METHOD(InitializeAnsi),
	EXPORT_METHOD(InitializeWide),
	EXPORT_METHOD(CompareTo),
	EXPORT_METHOD(GetDisplayNameEntry),
	EXPORT_METHOD(GetHashCode),
	EXPORT_METHOD(IsValid),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
