// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnRuntime/Private/Exports.hpp"

ENABLE_WARNINGS;

#define DECLARE_TYPE_EXPORT(TypeName) namespace Exports { namespace TypeName { FTypeSymbol Type(); } }
#define GET_TYPE_EXPORT(TypeName) Exports::TypeName::Type()

// Core
DECLARE_TYPE_EXPORT(Console);
DECLARE_TYPE_EXPORT(ConsoleObject);
DECLARE_TYPE_EXPORT(ConsoleVariable);
//DECLARE_TYPE_EXPORT(Internationalization);
DECLARE_TYPE_EXPORT(Log);
DECLARE_TYPE_EXPORT(Memory);
DECLARE_TYPE_EXPORT(MessageDialog);
DECLARE_TYPE_EXPORT(ModuleManager);
DECLARE_TYPE_EXPORT(Name);
DECLARE_TYPE_EXPORT(NameEntry);
DECLARE_TYPE_EXPORT(NameEntryHeader);
DECLARE_TYPE_EXPORT(NativeHashCode);
DECLARE_TYPE_EXPORT(Paths);
DECLARE_TYPE_EXPORT(ReferenceController);
DECLARE_TYPE_EXPORT(ScriptArray);
DECLARE_TYPE_EXPORT(ScriptBitArray);
DECLARE_TYPE_EXPORT(Text);

// CoreUObject
DECLARE_TYPE_EXPORT(ArrayProperty);
DECLARE_TYPE_EXPORT(BooleanProperty);
DECLARE_TYPE_EXPORT(ByteProperty);
DECLARE_TYPE_EXPORT(Class);
DECLARE_TYPE_EXPORT(ClassProperty);
DECLARE_TYPE_EXPORT(Classes);
DECLARE_TYPE_EXPORT(CompoundMember);
// DECLARE_TYPE_EXPORT(CppStruct);
DECLARE_TYPE_EXPORT(DelegateMethod);
DECLARE_TYPE_EXPORT(DelegateProperty);
DECLARE_TYPE_EXPORT(Enum);
DECLARE_TYPE_EXPORT(EnumProperty);
DECLARE_TYPE_EXPORT(InterfaceProperty);
DECLARE_TYPE_EXPORT(MapProperty);
DECLARE_TYPE_EXPORT(Member);
#if WITH_EDITOR
DECLARE_TYPE_EXPORT(MetaData);
#endif
DECLARE_TYPE_EXPORT(Method);
DECLARE_TYPE_EXPORT(MulticastDelegateProperty);
DECLARE_TYPE_EXPORT(Object);
DECLARE_TYPE_EXPORT(ObjectPropertyBase);
DECLARE_TYPE_EXPORT(Package);
DECLARE_TYPE_EXPORT(Property);
DECLARE_TYPE_EXPORT(PropertyClass);
DECLARE_TYPE_EXPORT(SetProperty);
DECLARE_TYPE_EXPORT(Struct);
DECLARE_TYPE_EXPORT(StructProperty);
DECLARE_TYPE_EXPORT(WeakObjectReference);

#if WITH_APPLICATION_CORE
// Slate
DECLARE_TYPE_EXPORT(BindingContext);
DECLARE_TYPE_EXPORT(Border);
DECLARE_TYPE_EXPORT(Box);
DECLARE_TYPE_EXPORT(Extender);
DECLARE_TYPE_EXPORT(GridPanelSlot);
DECLARE_TYPE_EXPORT(MenuBuilder);
DECLARE_TYPE_EXPORT(ToolBarBuilder);
DECLARE_TYPE_EXPORT(UICommandInfo);
DECLARE_TYPE_EXPORT(UICommandList);

// SlateCore
DECLARE_TYPE_EXPORT(HorizontalBoxSlot);
DECLARE_TYPE_EXPORT(Slot);
DECLARE_TYPE_EXPORT(VerticalBoxSlot);
#endif

#if UE_EDITOR
// LevelEditor
DECLARE_TYPE_EXPORT(LevelEditor);

// UnrealEd
DECLARE_TYPE_EXPORT(ExtensibilityManager);
#endif

namespace Exports
{
	const FTypeSymbol Types[] =
	{
		// Core
		GET_TYPE_EXPORT(Console),
		GET_TYPE_EXPORT(ConsoleObject),
		GET_TYPE_EXPORT(ConsoleVariable),
		//GET_TYPE_EXPORT(Internationalization),
		GET_TYPE_EXPORT(Log),
		GET_TYPE_EXPORT(Memory),
		GET_TYPE_EXPORT(MessageDialog),
		GET_TYPE_EXPORT(ModuleManager),
		GET_TYPE_EXPORT(Name),
		GET_TYPE_EXPORT(NameEntry),
		GET_TYPE_EXPORT(NameEntryHeader),
		GET_TYPE_EXPORT(NativeHashCode),
		GET_TYPE_EXPORT(Paths),
		GET_TYPE_EXPORT(ReferenceController),
		GET_TYPE_EXPORT(ScriptArray),
		GET_TYPE_EXPORT(ScriptBitArray),
		GET_TYPE_EXPORT(Text),

		// CoreUObject
		GET_TYPE_EXPORT(ArrayProperty),
		GET_TYPE_EXPORT(BooleanProperty),
		GET_TYPE_EXPORT(ByteProperty),
		GET_TYPE_EXPORT(Class),
		GET_TYPE_EXPORT(ClassProperty),
		GET_TYPE_EXPORT(Classes),
		GET_TYPE_EXPORT(CompoundMember),
		// GET_TYPE_EXPORT(CppStruct),
		GET_TYPE_EXPORT(DelegateMethod),
		GET_TYPE_EXPORT(DelegateProperty),
		GET_TYPE_EXPORT(Enum),
		GET_TYPE_EXPORT(EnumProperty),
		GET_TYPE_EXPORT(InterfaceProperty),
		GET_TYPE_EXPORT(MapProperty),
		GET_TYPE_EXPORT(Member),
#if WITH_EDITOR
		GET_TYPE_EXPORT(MetaData),
#endif
		GET_TYPE_EXPORT(Method),
		GET_TYPE_EXPORT(MulticastDelegateProperty),
		GET_TYPE_EXPORT(Object),
		GET_TYPE_EXPORT(ObjectPropertyBase),
		GET_TYPE_EXPORT(Package),
		GET_TYPE_EXPORT(Property),
		GET_TYPE_EXPORT(PropertyClass),
		GET_TYPE_EXPORT(SetProperty),
		GET_TYPE_EXPORT(Struct),
		GET_TYPE_EXPORT(StructProperty),
		GET_TYPE_EXPORT(WeakObjectReference),

#if WITH_APPLICATION_CORE
		// Slate
		GET_TYPE_EXPORT(BindingContext),
		GET_TYPE_EXPORT(Border),
		GET_TYPE_EXPORT(Box),
		GET_TYPE_EXPORT(Extender),
		GET_TYPE_EXPORT(GridPanelSlot),
		GET_TYPE_EXPORT(MenuBuilder),
		GET_TYPE_EXPORT(ToolBarBuilder),
		GET_TYPE_EXPORT(UICommandInfo),
		GET_TYPE_EXPORT(UICommandList),

		// SlateCore
		GET_TYPE_EXPORT(HorizontalBoxSlot),
		GET_TYPE_EXPORT(Slot),
		GET_TYPE_EXPORT(VerticalBoxSlot),
#endif

#if UE_EDITOR
		// LevelEditor
		GET_TYPE_EXPORT(LevelEditor),

		// UnrealEd
		GET_TYPE_EXPORT(ExtensibilityManager),
#endif
	};
}

const FTypeSymbol* GetTypeExports()
{
	return Exports::Types;
}

int32 GetTypeExportsLength()
{
	return UE_ARRAY_COUNT(Exports::Types);
}

RESTORE_WARNINGS;
