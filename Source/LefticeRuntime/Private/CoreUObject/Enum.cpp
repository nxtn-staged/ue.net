// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

#include "LefticeRuntime/Private/Exports.hpp"

#include "LefticeRuntime/Private/Accessor.hpp"
#include "LefticeRuntime/Private/Construct.hpp"
#include "LefticeRuntime/Private/MarshaledTypes.hpp"
#include "LefticeRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/CoreUObject/Public/UObject/Class.h"

ENABLE_WARNINGS;

DEFINE_OFFSET_ACCESSOR(UEnum, Names);

#define TYPE_EXPORT_MANAGED_TYPE_NAME Enum

TYPE_EXPORT_START

void EXPORT_CALL_CONV GetDisplayNameTextByIndex(const UEnum& Enum, int32_t Index, FText& OutResult)
{
	Construct(OutResult, Enum.GetDisplayNameTextByIndex(Index));
}

void EXPORT_CALL_CONV GetDisplayNameTextByValue(const UEnum& Enum, int64_t Value, FText& OutResult)
{
	Construct(OutResult, Enum.GetDisplayNameTextByValue(Value));
}

int32_t EXPORT_CALL_CONV GetIndexByName(const UEnum& Enum, FName Name)
{
	return Enum.GetIndexByName(Name);
}

int32_t EXPORT_CALL_CONV GetIndexByNameString(const UEnum& Enum, const FMarshaledString& Name)
{
	return Enum.GetIndexByNameString(Name.Data);
}

void EXPORT_CALL_CONV GetNameStringByIndex(const UEnum& Enum, int32_t Index, FMarshaledString& OutResult)
{
	OutResult = Enum.GetNameStringByIndex(Index);
}

#if WITH_EDITOR
bool EXPORT_CALL_CONV HasMetaData(const UEnum& Enum, const char16_t* Key, int32_t Index)
{
	return Enum.HasMetaData(CHAR16_TO_TCHAR(Key), Index);
}

bool EXPORT_CALL_CONV RemoveMetaData(UEnum& Enum, const char16_t* Key, int32_t Index)
{
	auto KeyConv = StringCast<TCHAR>(reinterpret_cast<const UTF16CHAR*>(Key));
	if (Enum.HasMetaData(KeyConv.Get(), Index))
	{
		Enum.RemoveMetaData(KeyConv.Get(), Index);
		return true;
	}

	return false;
}

void EXPORT_CALL_CONV SetMetaData(UEnum& Enum, const char16_t* Key, const char16_t* Value, int32_t Index)
{
	Enum.SetMetaData(CHAR16_TO_TCHAR(Key), CHAR16_TO_TCHAR(Value), Index);
}

bool EXPORT_CALL_CONV TryGetMetaData(const UEnum& Enum, const char16_t* Key, FMarshaledString& OutValue, int32_t Index)
{
	auto KeyConv = StringCast<TCHAR>(reinterpret_cast<const UTF16CHAR*>(Key));
	if (Enum.HasMetaData(KeyConv.Get(), Index))
	{
		OutValue = Enum.GetMetaData(KeyConv.Get(), Index);
		return true;
	}

	return false;
}
#endif

const FMemberSymbol Members[] =
{
	EXPORT_OFFSET(UEnum, Names, Names),

	EXPORT_METHOD(GetDisplayNameTextByIndex),
	EXPORT_METHOD(GetDisplayNameTextByValue),
	EXPORT_METHOD(GetIndexByName),
	EXPORT_METHOD(GetIndexByNameString),
	EXPORT_METHOD(GetNameStringByIndex),
#if WITH_EDITOR
	EXPORT_METHOD(HasMetaData),
	EXPORT_METHOD(RemoveMetaData),
	EXPORT_METHOD(SetMetaData),
	EXPORT_METHOD(TryGetMetaData),
#endif
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
