// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/CoreUObject/Public/UObject/Class.h"
#include "Runtime/CoreUObject/Public/UObject/EnumProperty.h"
#include "Runtime/CoreUObject/Public/UObject/FieldPathProperty.h"
#include "Runtime/CoreUObject/Public/UObject/Interface.h"
#include "Runtime/CoreUObject/Public/UObject/Package.h"
#include "Runtime/CoreUObject/Public/UObject/TextProperty.h"
#include "Runtime/CoreUObject/Public/UObject/UnrealType.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME Classes

TYPE_EXPORT_START

#define _EXPORT_CLASS(ManagedName, NativeName) EXPORT_SYMBOL(ManagedName, NativeName::StaticClass())
#define EXPORT_CLASS(ManagedName, NativeName) _EXPORT_CLASS(ManagedName##_Class, NativeName)
const FMemberSymbol Members[] =
{
	EXPORT_CLASS(Member, UField),
	EXPORT_CLASS(SByteProperty, FInt8Property),
	EXPORT_CLASS(Enum, UEnum),
	EXPORT_CLASS(CompoundMember, UStruct),
	EXPORT_CLASS(Struct, UScriptStruct),
	EXPORT_CLASS(Class, UClass),
	EXPORT_CLASS(ByteProperty, FByteProperty),
	EXPORT_CLASS(Int32Property, FIntProperty),
	EXPORT_CLASS(SingleProperty, FFloatProperty),
	EXPORT_CLASS(UInt64Property, FUInt64Property),
	EXPORT_CLASS(ClassProperty, FClassProperty),
	EXPORT_CLASS(UInt32Property, FUInt32Property),
	EXPORT_CLASS(InterfaceProperty, FInterfaceProperty),
	EXPORT_CLASS(NameProperty, FNameProperty),
	EXPORT_CLASS(StringProperty, FStrProperty),
	EXPORT_CLASS(Property, FProperty),
	EXPORT_CLASS(ObjectProperty, FObjectProperty),
	EXPORT_CLASS(BooleanProperty, FBoolProperty),
	EXPORT_CLASS(UInt16Property, FUInt16Property),
	EXPORT_CLASS(Method, UFunction),
	EXPORT_CLASS(StructProperty, FStructProperty),
	EXPORT_CLASS(ArrayProperty, FArrayProperty),
	EXPORT_CLASS(Int64Property, FInt64Property),
	EXPORT_CLASS(DelegateProperty, FDelegateProperty),
	EXPORT_CLASS(NumericProperty, FNumericProperty),
	EXPORT_CLASS(MulticastDelegateProperty, FMulticastDelegateProperty),
	EXPORT_CLASS(ObjectPropertyBase, FObjectPropertyBase),
	EXPORT_CLASS(WeakObjectProperty, FWeakObjectProperty),
	EXPORT_CLASS(LazyObjectProperty, FLazyObjectProperty),
	EXPORT_CLASS(SoftObjectProperty, FSoftObjectProperty),
	EXPORT_CLASS(TextProperty, FTextProperty),
	EXPORT_CLASS(Int16Property, FInt16Property),
	EXPORT_CLASS(DoubleProperty, FDoubleProperty),
	EXPORT_CLASS(SoftClassProperty, FSoftClassProperty),
	EXPORT_CLASS(Package, UPackage),
	EXPORT_CLASS(DelegateMethod, UDelegateFunction),
	EXPORT_CLASS(MapProperty, FMapProperty),
	EXPORT_CLASS(SetProperty, FSetProperty),
	EXPORT_CLASS(EnumProperty, FEnumProperty),
	EXPORT_CLASS(SparseDelegateMethod, USparseDelegateFunction),
	EXPORT_CLASS(MulticastInlineDelegateProperty, FMulticastInlineDelegateProperty),
	EXPORT_CLASS(MulticastSparseDelegateProperty, FMulticastSparseDelegateProperty),
	EXPORT_CLASS(PropertyPathProperty, FFieldPathProperty),

	EXPORT_CLASS(Interface, UInterface),
};
#undef _EXPORT_CLASS
#undef EXPORT_CLASS

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
