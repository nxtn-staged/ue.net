// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#pragma once

#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

ENABLE_WARNINGS;

#define CHAR16_TO_CHAR16(Str) reinterpret_cast<const UTF16CHAR*>(static_cast<const char16_t*>(Str))
#define CHAR16_TO_ANSI(Str) StringCast<ANSICHAR>(CHAR16_TO_CHAR16(Str)).Get()
#define CHAR16_TO_WIDE(Str) StringCast<WIDECHAR>(CHAR16_TO_CHAR16(Str)).Get()
#define CHAR16_TO_TCHAR(Str) StringCast<TCHAR>(CHAR16_TO_CHAR16(Str)).Get()

#ifdef AAA
struct FMemberSymbol
{
	const char16_t* Name;
	int32_t NameLength;
	void* Value;
};

#else
using FMemberSymbol = void*;
#endif

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

#define EXPORT_CALL_CONV STDCALL

#ifdef AAA
template<int32_t FieldNameSize>
constexpr FMemberSymbol ExportField(const char16_t (&FieldName)[FieldNameSize], void* FieldPtr)
{
	return FMemberSymbol{ FieldName, FieldNameSize - 1, FieldPtr };
}

#define EXPORT_FIELD(FieldName, FieldPtr) ExportField(u###FieldName, FieldPtr)

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

#define EXPORT_SYMBOL(SymbolName, SymbolValue) ExportSymbol(u###SymbolName, SymbolValue)
#else
#define EXPORT_FIELD(FieldName, FieldPtr) reinterpret_cast<void*>(FieldPtr)
#define EXPORT_METHOD(MethodName) reinterpret_cast<void*>(MethodName)
#define EXPORT_OFFSET(TypeName, ManagedName, NativeName) reinterpret_cast<void*>(OFFSETOF(TypeName, NativeName))
#define EXPORT_OFFSET_PUBLIC(TypeName, ManagedName, NativeName) reinterpret_cast<void*>(OFFSETOF_PUBLIC(TypeName, NativeName))
#define EXPORT_SYMBOL(SymbolName, SymbolValue) reinterpret_cast<void*>(SymbolValue)
#endif

const FTypeSymbol* GetTypeExports();

int32_t GetTypeExportsLength();

RESTORE_WARNINGS;
