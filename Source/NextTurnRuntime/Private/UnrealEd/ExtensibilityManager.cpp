// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#if UE_EDITOR

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Editor/UnrealEd/Public/Toolkits/AssetEditorToolkit.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME ExtensibilityManager

TYPE_EXPORT_START

void EXPORT_CALL_CONV AddExtender(const TSharedRef<FExtensibilityManager>& Manager, const TSharedRef<FExtender>& Extender)
{
	Manager->AddExtender(Extender);
}

void EXPORT_CALL_CONV RemoveExtender(const TSharedRef<FExtensibilityManager>& Manager, const TSharedRef<FExtender>& Extender)
{
	Manager->RemoveExtender(Extender);
}

const FMemberSymbol Members[] =
{
	EXPORT_METHOD(AddExtender),
	EXPORT_METHOD(RemoveExtender),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;

#endif
