// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

#if WITH_EDITOR

#include "LefticeRuntime/Private/Exports.hpp"

#include "LefticeRuntime/Private/MarshaledTypes.hpp"
#include "LefticeRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/CoreUObject/Public/UObject/MetaData.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME MetaData

TYPE_EXPORT_START

#if WITH_EDITOR
FORCEINLINE bool EXPORT_CALL_CONV _HasValue(UMetaData& MetaData, const UObject* Object, const FName& Key)
{
	return MetaData.HasValue(Object, Key);
}

bool EXPORT_CALL_CONV HasValue(UMetaData& MetaData, const UObject* Object, const char16_t* Key)
{
	return _HasValue(MetaData, Object, FName{ CHAR16_TO_TCHAR(Key), FNAME_Find });
}

bool EXPORT_CALL_CONV HasValueWithName(UMetaData& MetaData, const UObject* Object, const FMarshaledName Key)
{
	return _HasValue(MetaData, Object, Key.ToName());
}

FORCEINLINE bool EXPORT_CALL_CONV _RemoveValue(UMetaData& MetaData, const UObject* Object, const FName& Key)
{
	if (MetaData.HasValue(Object, Key))
	{
		MetaData.RemoveValue(Object, Key);
		return true;
	}

	return false;
}

bool EXPORT_CALL_CONV RemoveValue(UMetaData& MetaData, const UObject* Object, const char16_t* Key)
{
	return _RemoveValue(MetaData, Object, FName{ CHAR16_TO_TCHAR(Key), FNAME_Find });
}

bool EXPORT_CALL_CONV RemoveValueWithName(UMetaData& MetaData, UObject* Object, const FMarshaledName Key)
{
	return _RemoveValue(MetaData, Object, Key.ToName());
}

FORCEINLINE void EXPORT_CALL_CONV _SetValue(UMetaData& MetaData, UObject* Object, const FName& Key, const char16_t* Value)
{
	MetaData.SetValue(Object, Key, CHAR16_TO_TCHAR(Value));
}

void EXPORT_CALL_CONV SetValue(UMetaData& MetaData, UObject* Object, const char16_t* Key, const char16_t* Value)
{
	_SetValue(MetaData, Object, FName{ CHAR16_TO_TCHAR(Key) }, Value);
}

void EXPORT_CALL_CONV SetValueWithName(UMetaData& MetaData, UObject* Object, const FMarshaledName Key, const char16_t* Value)
{
	_SetValue(MetaData, Object, Key.ToName(), Value);
}

FORCEINLINE bool EXPORT_CALL_CONV _TryGetValue(UMetaData& MetaData, const UObject* Object, const FName& Key, FMarshaledString& OutValue)
{
	if (MetaData.HasValue(Object, Key))
	{
		OutValue = MetaData.GetValue(Object, Key);
		return true;
	}

	return false;
}

bool EXPORT_CALL_CONV TryGetValue(UMetaData& MetaData, const UObject* Object, const char16_t* Key, FMarshaledString& OutValue)
{
	return _TryGetValue(MetaData, Object, FName{ CHAR16_TO_TCHAR(Key), FNAME_Find }, OutValue);
}

bool EXPORT_CALL_CONV TryGetValueWithName(UMetaData& MetaData, const UObject* Object, const FMarshaledName Key, FMarshaledString& OutValue)
{
	return _TryGetValue(MetaData, Object, Key.ToName(), OutValue);
}
#endif

const FMemberSymbol Members[] =
{
	EXPORT_METHOD(HasValue),
	EXPORT_METHOD(HasValueWithName),
	EXPORT_METHOD(RemoveValue),
	EXPORT_METHOD(RemoveValueWithName),
	EXPORT_METHOD(SetValue),
	EXPORT_METHOD(SetValueWithName),
	EXPORT_METHOD(TryGetValue),
	EXPORT_METHOD(TryGetValueWithName),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;

#endif
