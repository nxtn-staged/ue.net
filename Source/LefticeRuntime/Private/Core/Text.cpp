// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

#include "LefticeRuntime/Private/Exports.hpp"

#include "LefticeRuntime/Private/Construct.hpp"
#include "LefticeRuntime/Private/MarshaledTypes.hpp"
#include "LefticeRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME Text

TYPE_EXPORT_START

void EXPORT_CALL_CONV InitializeFromName(FText& Text, const FMarshaledName Value)
{
	Construct(Text, FText::FromName(Value.ToName()));
}

void EXPORT_CALL_CONV InitializeFromString(FText& Text, const FMarshaledString Value)
{
	Construct(Text, FText::FromString(Value.Data));
}

int32_t EXPORT_CALL_CONV CompareTo(const FText& Text, const FText& Other, ETextComparisonLevel::Type Level)
{
	return Text.CompareTo(Other, Level);
}

void EXPORT_CALL_CONV GetEmpty(FText& Text)
{
	Construct(Text, FText::GetEmpty());
}

bool EXPORT_CALL_CONV IsEmpty(const FText& Text)
{
	return Text.IsEmpty();
}

bool EXPORT_CALL_CONV IsNumeric(const FText& Text)
{
	return Text.IsNumeric();
}

bool EXPORT_CALL_CONV IsWhiteSpace(const FText& Text)
{
	return Text.IsEmptyOrWhitespace();
}

void EXPORT_CALL_CONV ToLower(const FText& Text, FText& OutResult)
{
	Construct(OutResult, Text.ToLower());
}

void EXPORT_CALL_CONV ToString(const FText& Text, FMarshaledString& OutResult)
{
	OutResult = Text.ToString();
}

void EXPORT_CALL_CONV ToUpper(const FText& Text, FText& OutResult)
{
	Construct(OutResult, Text.ToUpper());
}

void EXPORT_CALL_CONV Trim(const FText& Text, FText& OutResult)
{
	Construct(OutResult, FText::TrimPrecedingAndTrailing(Text));
}

void EXPORT_CALL_CONV TrimEnd(const FText& Text, FText& OutResult)
{
	Construct(OutResult, FText::TrimTrailing(Text));
}

void EXPORT_CALL_CONV TrimStart(const FText& Text, FText& OutResult)
{
	Construct(OutResult, FText::TrimPreceding(Text));
}

const FMemberSymbol Members[] =
{
	EXPORT_METHOD(InitializeFromName),
	EXPORT_METHOD(InitializeFromString),
	EXPORT_METHOD(CompareTo),
	EXPORT_METHOD(GetEmpty),
	EXPORT_METHOD(IsEmpty),
	EXPORT_METHOD(IsNumeric),
	EXPORT_METHOD(IsWhiteSpace),
	EXPORT_METHOD(ToLower),
	EXPORT_METHOD(ToString),
	EXPORT_METHOD(ToUpper),
	EXPORT_METHOD(Trim),
	EXPORT_METHOD(TrimEnd),
	EXPORT_METHOD(TrimStart),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
