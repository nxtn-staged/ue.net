// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Private/Accessor.hpp"
#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/CoreUObject/Public/UObject/Class.h"

ENABLE_WARNINGS;

DEFINE_OFFSET_ACCESSOR(UStruct, SuperStruct);

#define TYPE_EXPORT_MANAGED_TYPE_NAME CompoundMember

TYPE_EXPORT_START

const TCHAR* EXPORT_CALL_CONV GetCppPrefix(const UStruct& Member)
{
	return Member.GetPrefixCPP();
}

UStruct* EXPORT_CALL_CONV GetInheritanceBaseMember(const UStruct& Member)
{
	return Member.GetInheritanceSuper();
}

const FMemberSymbol Members[] =
{
	EXPORT_OFFSET(UStruct, BaseMember, SuperStruct),
	EXPORT_OFFSET_PUBLIC(UStruct, FirstMember, Children),
	EXPORT_OFFSET_PUBLIC(UStruct, FirstProperty, ChildProperties),

	EXPORT_METHOD(GetCppPrefix),
	EXPORT_METHOD(GetInheritanceBaseMember),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
