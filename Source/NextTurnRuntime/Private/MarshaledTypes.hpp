// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#pragma once

#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

ENABLE_WARNINGS;

struct FMarshaledName
{
#if WITH_CASE_PRESERVING_NAME
	FName Data;

	const FName& ToName() const
	{
		return Data;
	}

	FMarshaledName& operator=(const FName& Other)
	{
		Data = Other;
		return *this;
	}
#else
	FScriptName Data;

	FName ToName() const
	{
		return ScriptNameToName(Data);
	}

	FMarshaledName& operator=(const FName& Other)
	{
		Data = NameToScriptName(Other);
		return *this;
	}
#endif
};

struct FMarshaledString
{
	FString Data;

	FMarshaledString& operator=(const FString& Other)
	{
		new(&Data) FString{ Other };
		return *this;
	}

	FMarshaledString& operator=(FString&& Other)
	{
		new(&Data) FString{ Other };
		return *this;
	}
};

RESTORE_WARNINGS;
