// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

#include "LefticeRuntime/Private/Exports.hpp"

#include "LefticeRuntime/Private/Accessor.hpp"
#include "LefticeRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/CoreUObject/Public/UObject/Class.h"

ENABLE_WARNINGS;

DEFINE_OFFSET_ACCESSOR(UStruct, SuperStruct);
DEFINE_OFFSET_ACCESSOR(UStruct, Children);

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
	EXPORT_OFFSET(UStruct, FirstMember, Children),

	EXPORT_METHOD(GetCppPrefix),
	EXPORT_METHOD(GetInheritanceBaseMember),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
