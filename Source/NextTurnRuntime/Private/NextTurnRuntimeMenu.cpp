// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#if UE_EDITOR

#include "NextTurnRuntimeMenu.hpp"

#include "NextTurnRuntimeCommands.hpp"

#include "Runtime/Core/Public/Modules/ModuleManager.h"

#include "Runtime/Slate/Public/Framework/MultiBox/MultiBoxBuilder.h"
#include "Runtime/Slate/Public/Framework/MultiBox/MultiBoxExtender.h"

#include "Editor/LevelEditor/Public/LevelEditor.h"

#include "Editor/UnrealEd/Public/Toolkits/AssetEditorToolkit.h"

ENABLE_WARNINGS;

namespace NextTurnRuntimeMenu
{
	void BuildMenu(FMenuBuilder& Menu)
	{
		const FNextTurnRuntimeCommands& Commands = FNextTurnRuntimeCommands::Get();

		Menu.BeginSection("UENETLoad", INVTEXT("Load"));
		{
			Menu.AddMenuEntry(Commands.Load);
			Menu.AddMenuEntry(Commands.Reload);
			Menu.AddMenuEntry(Commands.Unload);
		}

		Menu.EndSection();
	}

	void ExtendMenuBar(FMenuBarBuilder& MenuBar)
	{
		MenuBar.AddPullDownMenu(INVTEXT("UE.NET"), INVTEXT("UE.NET"), FNewMenuDelegate::CreateStatic(BuildMenu), "UENET");
	}

	void MakeMenu(const TSharedRef<FUICommandList>& CommandList)
	{
		TSharedRef<FExtender> Extender{ new FExtender{} };

		Extender->AddMenuBarExtension(
			"Help",
			EExtensionHook::After,
			CommandList,
			FMenuBarExtensionDelegate::CreateStatic(ExtendMenuBar));

		 FModuleManager::Get().LoadModuleChecked<FLevelEditorModule>("LevelEditor")
		 	.GetMenuExtensibilityManager()->AddExtender(Extender);
	}
}

RESTORE_WARNINGS;

#endif
