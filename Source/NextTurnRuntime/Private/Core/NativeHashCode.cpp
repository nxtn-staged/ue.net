// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME NativeHashCode

TYPE_EXPORT_START

//int32_t ForCharArray(const char16_t* Value)
//{
//}

int32_t EXPORT_CALL_CONV ForGuid(const FGuid& Value)
{
	return static_cast<int32_t>(GetTypeHash(Value));
}

int32_t EXPORT_CALL_CONV ForPointer(const void* Value)
{
	return static_cast<int32_t>(GetTypeHash(Value));
}

const FMemberSymbol Members[] =
{
	//EXPORT_METHOD(ForCharArray),
	EXPORT_METHOD(ForGuid),
	EXPORT_METHOD(ForPointer),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
