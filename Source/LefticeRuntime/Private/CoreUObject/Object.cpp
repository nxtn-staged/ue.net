// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

#include "Exports.hpp"

#include "LefticeRuntime/Private/Accessor.hpp"
#include "LefticeRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/CoreUObject/Public/UObject/Class.h"

ENABLE_WARNINGS;

DEFINE_OFFSET_ACCESSOR(UObjectBase, ClassPrivate);
DEFINE_OFFSET_ACCESSOR(UObjectBase, NamePrivate);

#define TYPE_EXPORT_MANAGED_TYPE_NAME Object

TYPE_EXPORT_START

UPackage* EXPORT_CALL_CONV GetPackage(const UObject& Object)
{
	return Object.GetOutermost();
}

void EXPORT_CALL_CONV InvokeMethod(UObject& Object, UFunction& Method, void* Parameters)
{
	Object.ProcessEvent(&Method, Parameters);
}

const FMemberSymbol Members[] =
{
	EXPORT_OFFSET(UObjectBase, Class, ClassPrivate),
	EXPORT_OFFSET(UObjectBase, Name, NamePrivate),

	EXPORT_METHOD(GetPackage),
	EXPORT_METHOD(InvokeMethod),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
