// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#if WITH_APPLICATION_CORE

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Private/Construct.hpp"
#include "NextTurnRuntime/Private/ManagedDelegate.hpp"
#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/Slate/Public/Framework/Commands/UICommandList.h"
#include "Runtime/Slate/Public/Framework/MultiBox/MultiBoxExtender.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME Extender

TYPE_EXPORT_START

void EXPORT_CALL_CONV Initialize(TSharedRef<FExtender>& Extender)
{
	Construct(Extender, TSharedRef<FExtender>{ new FExtender{} });
}

void EXPORT_CALL_CONV AddMenuBarExtension(
	const TSharedRef<FExtender>& Extender,
	FName ExtensionPoint,
	EExtensionHook::Position Position,
	const TSharedRef<FUICommandList>& CommandList,
	FMenuBarExtensionDelegate::FStaticDelegate::FFuncPtr ExtendMenuBar,
	intptr_t ExtendMenuBarHandle,
	TSharedRef<const FExtensionBase>& Extension)
{
	Construct(Extension, Extender->AddMenuBarExtension(
		ExtensionPoint,
		Position,
		CommandList,
		TManagedDelegate<FMenuBarExtensionDelegate>::Create(ExtendMenuBar, ExtendMenuBarHandle)));
}

void EXPORT_CALL_CONV AddMenuExtension(
	const TSharedRef<FExtender>& Extender,
	FName ExtensionPoint,
	EExtensionHook::Position Position,
	const TSharedRef<FUICommandList>& CommandList,
	FMenuExtensionDelegate::FStaticDelegate::FFuncPtr ExtendMenu,
	intptr_t ExtendMenuHandle,
	TSharedRef<const FExtensionBase>& Extension)
{
	Construct(Extension, Extender->AddMenuExtension(
		ExtensionPoint,
		Position,
		CommandList,
		TManagedDelegate<FMenuExtensionDelegate>::Create(ExtendMenu, ExtendMenuHandle)));
}

void EXPORT_CALL_CONV AddToolBarExtension(
	const TSharedRef<FExtender>& Extender,
	FName ExtensionPoint,
	EExtensionHook::Position Position,
	const TSharedRef<FUICommandList>& CommandList,
	FToolBarExtensionDelegate::FStaticDelegate::FFuncPtr ExtendToolBar,
	intptr_t ExtendToolBarHandle,
	TSharedRef<const FExtensionBase>& Extension)
{
	Construct(Extension, Extender->AddToolBarExtension(
		ExtensionPoint,
		Position,
		CommandList,
		TManagedDelegate<FToolBarExtensionDelegate>::Create(ExtendToolBar, ExtendToolBarHandle)));
}

const FMemberSymbol Members[] =
{
	EXPORT_METHOD(Initialize),
	EXPORT_METHOD(AddMenuBarExtension),
	EXPORT_METHOD(AddMenuExtension),
	EXPORT_METHOD(AddToolBarExtension),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;

#endif
