// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

#pragma once

#include "LefticeRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

ENABLE_WARNINGS;

#define CHAR16_TO_CHAR16(Str) reinterpret_cast<const UTF16CHAR*>(static_cast<const char16_t*>(Str))
#define CHAR16_TO_ANSI(Str) StringCast<ANSICHAR>(CHAR16_TO_CHAR16(Str)).Get()
#define CHAR16_TO_WIDE(Str) StringCast<WIDECHAR>(CHAR16_TO_CHAR16(Str)).Get()
#define CHAR16_TO_TCHAR(Str) StringCast<TCHAR>(CHAR16_TO_CHAR16(Str)).Get()

struct FMemberSymbol
{
	const char16_t* Name;
	int32_t NameLength;
	void* Value;
};

struct FTypeSymbol
{
	const char16_t* Name;
	int32_t NameLength;
	const FMemberSymbol* Members;
	int32_t MembersLength;
};

template<int32_t TypeNameSize, int32_t MembersLength>
constexpr FTypeSymbol ExportType(const char16_t (&TypeName)[TypeNameSize], const FMemberSymbol (&MemberEntries)[MembersLength])
{
	return FTypeSymbol{ TypeName, TypeNameSize - 1, MemberEntries, MembersLength };
}

#define DEFINE_TYPE_EXPORT(TypeName) FTypeSymbol Type() { return ExportType(u###TypeName, Members); }

#define TYPE_EXPORT_START namespace Exports { namespace TYPE_EXPORT_MANAGED_TYPE_NAME {

#define	_TYPE_EXPORT_END(TypeName) DEFINE_TYPE_EXPORT(TypeName); } }
#define	TYPE_EXPORT_END _TYPE_EXPORT_END(TYPE_EXPORT_MANAGED_TYPE_NAME)

template<int32_t FieldNameSize>
constexpr FMemberSymbol ExportField(const char16_t (&FieldName)[FieldNameSize], void* FieldPtr)
{
	return FMemberSymbol{ FieldName, FieldNameSize - 1, FieldPtr };
}

#define EXPORT_CALL_CONV STDCALL

template<int32_t MethodNameSize, typename ResultType, typename... ParameterTypes>
inline FMemberSymbol ExportMethod(const char16_t (&MethodName)[MethodNameSize], ResultType (EXPORT_CALL_CONV *MethodPtr)(ParameterTypes...))
{
	return FMemberSymbol{ MethodName, MethodNameSize - 1, reinterpret_cast<void*>(MethodPtr) };
}

#define EXPORT_METHOD(MethodName) ExportMethod(u###MethodName, MethodName)

template<int32_t PropertyNameSize>
inline FMemberSymbol ExportOffset(const char16_t (&PropertyName)[PropertyNameSize], size_t PropertyOffset)
{
	return FMemberSymbol{ PropertyName, PropertyNameSize - 1, reinterpret_cast<void*>(PropertyOffset) };
}

#define EXPORT_OFFSET(TypeName, ManagedName, NativeName) ExportOffset(u###ManagedName, OFFSETOF(TypeName, NativeName))

template<int32_t SymbolNameSize, typename SymbolValueType>
FMemberSymbol ExportSymbol(const char16_t (&SymbolName)[SymbolNameSize], SymbolValueType SymbolValue)
{
	return FMemberSymbol{ SymbolName, SymbolNameSize - 1, reinterpret_cast<void*>(SymbolValue) };
}

const FTypeSymbol* GetTypeExports();

int32_t GetTypeExportsLength();

RESTORE_WARNINGS;
