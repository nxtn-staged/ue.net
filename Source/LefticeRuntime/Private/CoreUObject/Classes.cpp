// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

#include "LefticeRuntime/Private/Exports.hpp"

#include "LefticeRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/CoreUObject/Public/UObject/Class.h"
#include "Runtime/CoreUObject/Public/UObject/EnumProperty.h"
#include "Runtime/CoreUObject/Public/UObject/Package.h"
#include "Runtime/CoreUObject/Public/UObject/TextProperty.h"
#include "Runtime/CoreUObject/Public/UObject/UnrealType.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME Classes

TYPE_EXPORT_START

#define _EXPORT_CLASS(ManagedName, NativeName) ExportSymbol(u###ManagedName, NativeName::StaticClass())
#define EXPORT_CLASS(ManagedName, NativeName) _EXPORT_CLASS(ManagedName##_Class, NativeName)
const FMemberSymbol Members[] =
{
	EXPORT_CLASS(Member, UField),
	EXPORT_CLASS(SByteProperty, UInt8Property),
	EXPORT_CLASS(Enum, UEnum),
	EXPORT_CLASS(CompoundMember, UStruct),
	EXPORT_CLASS(Struct, UScriptStruct),
	EXPORT_CLASS(Class, UClass),
	EXPORT_CLASS(ByteProperty, UByteProperty),
	EXPORT_CLASS(Int32Property, UIntProperty),
	EXPORT_CLASS(SingleProperty, UFloatProperty),
	EXPORT_CLASS(UInt64Property, UUInt64Property),
	EXPORT_CLASS(ClassProperty, UClassProperty),
	EXPORT_CLASS(UInt32Property, UUInt32Property),
	EXPORT_CLASS(InterfaceProperty, UInterfaceProperty),
	EXPORT_CLASS(NameProperty, UNameProperty),
	EXPORT_CLASS(StringProperty, UStrProperty),
	EXPORT_CLASS(Property, UProperty),
	EXPORT_CLASS(ObjectProperty, UObjectProperty),
	EXPORT_CLASS(BooleanProperty, UBoolProperty),
	EXPORT_CLASS(UInt16Property, UUInt16Property),
	EXPORT_CLASS(Method, UFunction),
	EXPORT_CLASS(StructProperty, UStructProperty),
	EXPORT_CLASS(ArrayProperty, UArrayProperty),
	EXPORT_CLASS(Int64Property, UInt64Property),
	EXPORT_CLASS(DelegateProperty, UDelegateProperty),
	EXPORT_CLASS(NumericProperty, UNumericProperty),
	EXPORT_CLASS(MulticastDelegateProperty, UMulticastDelegateProperty),
	EXPORT_CLASS(ObjectPropertyBase, UObjectPropertyBase),
	EXPORT_CLASS(WeakObjectProperty, UWeakObjectProperty),
	EXPORT_CLASS(LazyObjectProperty, ULazyObjectProperty),
	EXPORT_CLASS(SoftObjectProperty, USoftObjectProperty),
	EXPORT_CLASS(TextProperty, UTextProperty),
	EXPORT_CLASS(Int16Property, UInt16Property),
	EXPORT_CLASS(DoubleProperty, UDoubleProperty),
	EXPORT_CLASS(SoftClassProperty, USoftClassProperty),
	EXPORT_CLASS(Package, UPackage),
	EXPORT_CLASS(DelegateMethod, UDelegateFunction),
	EXPORT_CLASS(MapProperty, UMapProperty),
	EXPORT_CLASS(SetProperty, USetProperty),
	EXPORT_CLASS(EnumProperty, UEnumProperty),
	EXPORT_CLASS(SparseDelegateMethod, USparseDelegateFunction),
	EXPORT_CLASS(MulticastInlineDelegateProperty, UMulticastInlineDelegateProperty),
	EXPORT_CLASS(MulticastSparseDelegateProperty, UMulticastSparseDelegateProperty),
};
#undef _EXPORT_CLASS
#undef EXPORT_CLASS

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
