// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

#include "LefticeRuntime/Private/Exports.hpp"

#include "LefticeRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME DelegateMethod

TYPE_EXPORT_START

void EXPORT_CALL_CONV GetNameSuffix(const TCHAR*& SuffixPtr, int32_t& SuffixLength)
{
	SuffixPtr = HEADER_GENERATED_DELEGATE_SIGNATURE_SUFFIX;
	SuffixLength = ARRAY_COUNT(HEADER_GENERATED_DELEGATE_SIGNATURE_SUFFIX) - 1;
}

const FMemberSymbol Members[] =
{
	EXPORT_METHOD(GetNameSuffix),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
