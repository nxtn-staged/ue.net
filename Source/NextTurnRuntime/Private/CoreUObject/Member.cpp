// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Private/Accessor.hpp"
#include "NextTurnRuntime/Private/MarshaledTypes.hpp"
#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/CoreUObject/Public/UObject/Class.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME Member

TYPE_EXPORT_START

#if WITH_EDITOR
FORCEINLINE bool EXPORT_CALL_CONV _HasMetaData(const UField& Member, const FName& Key)
{
	return Member.HasMetaData(Key);
}

bool EXPORT_CALL_CONV HasMetaData(const UField& Member, const char16_t* Key)
{
	return _HasMetaData(Member, FName{ CHAR16_TO_TCHAR(Key), FNAME_Find });
}

bool EXPORT_CALL_CONV HasMetaDataWithName(const UField& Member, const FMarshaledName Key)
{
	return _HasMetaData(Member, Key.ToName());
}

FORCEINLINE bool EXPORT_CALL_CONV _RemoveMetaData(UField& Member, const FName& Key)
{
	if (Member.HasMetaData(Key))
	{
		Member.RemoveMetaData(Key);
		return true;
	}

	return false;
}

bool EXPORT_CALL_CONV RemoveMetaData(UField& Member, const char16_t* Key)
{
	return _RemoveMetaData(Member, FName{ CHAR16_TO_TCHAR(Key), FNAME_Find });
}

bool EXPORT_CALL_CONV RemoveMetaDataWithName(UField& Member, const FMarshaledName Key)
{
	return _RemoveMetaData(Member, Key.ToName());
}

FORCEINLINE void EXPORT_CALL_CONV _SetMetaData(UField& Member, const FName& Key, const char16_t* Value)
{
	Member.SetMetaData(Key, CHAR16_TO_TCHAR(Value));
}

void EXPORT_CALL_CONV SetMetaData(UField& Member, const char16_t* Key, const char16_t* Value)
{
	_SetMetaData(Member, FName{ CHAR16_TO_TCHAR(Key) }, Value);
}

void EXPORT_CALL_CONV SetMetaDataWithName(UField& Member, const FMarshaledName Key, const char16_t* Value)
{
	_SetMetaData(Member, Key.ToName(), Value);
}

FORCEINLINE bool EXPORT_CALL_CONV _TryGetMetaData(const UField& Member, const FName& Key, FMarshaledString& OutValue)
{
	if (Member.HasMetaData(Key))
	{
		OutValue = Member.GetMetaData(Key);
		return true;
	}

	return false;
}

bool EXPORT_CALL_CONV TryGetMetaData(const UField& Member, const char16_t* Key, FMarshaledString& OutValue)
{
	return _TryGetMetaData(Member, FName{ CHAR16_TO_TCHAR(Key), FNAME_Find }, OutValue);
}

bool EXPORT_CALL_CONV TryGetMetaDataWithName(const UField& Member, const FMarshaledName Key, FMarshaledString& OutValue)
{
	return _TryGetMetaData(Member, Key.ToName(), OutValue);
}
#endif

const FMemberSymbol Members[] =
{
	EXPORT_OFFSET_PUBLIC(UField, Next, Next),

#if WITH_EDITOR
	EXPORT_METHOD(HasMetaData),
	EXPORT_METHOD(HasMetaDataWithName),
	EXPORT_METHOD(RemoveMetaData),
	EXPORT_METHOD(RemoveMetaDataWithName),
	EXPORT_METHOD(SetMetaData),
	EXPORT_METHOD(SetMetaDataWithName),
	EXPORT_METHOD(TryGetMetaData),
	EXPORT_METHOD(TryGetMetaDataWithName),
#endif
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
