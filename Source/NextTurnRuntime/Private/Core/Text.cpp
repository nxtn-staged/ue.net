// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Private/Construct.hpp"
#include "NextTurnRuntime/Private/MarshaledTypes.hpp"
#include "NextTurnRuntime/Public/Compiler.hpp"

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

class _FText
{
	virtual auto EXPORT_CALL_CONV InitializeFromName(FText& Text, const FMarshaledName Value) -> void;

	virtual auto EXPORT_CALL_CONV InitializeFromString(FText& Text, const FMarshaledString Value) -> void;

	virtual auto EXPORT_CALL_CONV CompareTo(const FText& Text, const FText& Other, ETextComparisonLevel::Type Level) -> int32_t;

	virtual auto EXPORT_CALL_CONV GetEmpty(FText& Text) -> void;

	virtual auto EXPORT_CALL_CONV IsEmpty(const FText& Text) -> bool;

	virtual auto EXPORT_CALL_CONV IsNumeric(const FText& Text) -> bool;

	virtual auto EXPORT_CALL_CONV IsWhiteSpace(const FText& Text) -> bool;

	virtual auto EXPORT_CALL_CONV ToLower(const FText& Text, FText& OutResult) -> void;

	virtual auto EXPORT_CALL_CONV ToString(const FText& Text, FMarshaledString& OutResult) -> void;

	virtual auto EXPORT_CALL_CONV ToUpper(const FText& Text, FText& OutResult) -> void;

	virtual auto EXPORT_CALL_CONV Trim(const FText& Text, FText& OutResult) -> void;

	virtual auto EXPORT_CALL_CONV TrimEnd(const FText& Text, FText& OutResult) -> void;

	virtual auto EXPORT_CALL_CONV TrimStart(const FText& Text, FText& OutResult) -> void;
};

class _FText
{
	virtual auto EXPORT_CALL_CONV InitializeFromName(FText& Text, const FMarshaledName Value) -> void
	{
		Construct(Text, FText::FromName(Value.ToName()));
	}

	virtual auto EXPORT_CALL_CONV InitializeFromString(FText& Text, const FMarshaledString Value) -> void
	{
		Construct(Text, FText::FromString(Value.Data));
	}

	virtual auto EXPORT_CALL_CONV CompareTo(const FText& Text, const FText& Other, ETextComparisonLevel::Type Level) -> int32_t
	{
		return Text.CompareTo(Other, Level);
	}

	virtual auto EXPORT_CALL_CONV GetEmpty(FText& Text) -> void
	{
		Construct(Text, FText::GetEmpty());
	}

	virtual auto EXPORT_CALL_CONV IsEmpty(const FText& Text) -> bool
	{
		return Text.IsEmpty();
	}

	virtual auto EXPORT_CALL_CONV IsNumeric(const FText& Text) -> bool
	{
		return Text.IsNumeric();
	}

	virtual auto EXPORT_CALL_CONV IsWhiteSpace(const FText& Text) -> bool
	{
		return Text.IsEmptyOrWhitespace();
	}

	virtual auto EXPORT_CALL_CONV ToLower(const FText& Text, FText& OutResult) -> void
	{
		Construct(OutResult, Text.ToLower());
	}

	virtual auto EXPORT_CALL_CONV ToString(const FText& Text, FMarshaledString& OutResult) -> void
	{
		OutResult = Text.ToString();
	}

	virtual auto EXPORT_CALL_CONV ToUpper(const FText& Text, FText& OutResult) -> void
	{
		Construct(OutResult, Text.ToUpper());
	}

	virtual auto EXPORT_CALL_CONV Trim(const FText& Text, FText& OutResult) -> void
	{
		Construct(OutResult, FText::TrimPrecedingAndTrailing(Text));
	}

	virtual auto EXPORT_CALL_CONV TrimEnd(const FText& Text, FText& OutResult) -> void
	{
		Construct(OutResult, FText::TrimTrailing(Text));
	}

	virtual auto EXPORT_CALL_CONV TrimStart(const FText& Text, FText& OutResult) -> void
	{
		Construct(OutResult, FText::TrimPreceding(Text));
	}
};

RESTORE_WARNINGS;
