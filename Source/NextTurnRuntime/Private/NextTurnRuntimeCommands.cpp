// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#if UE_EDITOR

#include "NextTurnRuntimeCommands.hpp"

#include "Editor/EditorStyle/Public/EditorStyle.h"

ENABLE_WARNINGS;

#define LOCTEXT_NAMESPACE "NextTurnRuntime"

FNextTurnRuntimeCommands::FNextTurnRuntimeCommands() : TCommands<FNextTurnRuntimeCommands>
{
	"NextTurnRuntime", INVTEXT("NextTurn Runtime"), NAME_None, FEditorStyle::GetStyleSetName()
}
{
}

void FNextTurnRuntimeCommands::RegisterCommands()
{
	UI_COMMAND(Load, "Load", "Load", EUserInterfaceActionType::Button, FInputChord{});
	UI_COMMAND(Reload, "Reload", "Reload", EUserInterfaceActionType::Button, FInputChord{});
	UI_COMMAND(Unload, "Unload", "Unload", EUserInterfaceActionType::Button, FInputChord{});
}

#undef LOCTEXT_NAMESPACE

RESTORE_WARNINGS;

#endif
