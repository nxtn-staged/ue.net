// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

#if UE_EDITOR

#include "LefticeRuntime/Private/Exports.hpp"

#include "LefticeRuntime/Private/Construct.hpp"
#include "LefticeRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"
#include "Runtime/Core/Public/Modules/ModuleManager.h"

#include "Runtime/Slate/Public/Widgets/Layout/SBox.h"

#include "Runtime/SlateCore/Public/Widgets/SWidget.h"

#include "Editor/LevelEditor/Private/SLevelEditor.h"
#include "Editor/LevelEditor/Public/ILevelEditor.h"
#include "Editor/LevelEditor/Public/LevelEditor.h"

#include "Editor/MainFrame/Public/Interfaces/IMainFrameModule.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME LevelEditor

TYPE_EXPORT_START

void EXPORT_CALL_CONV GetMenuExtensibilityManager(FLevelEditorModule& Editor, TSharedRef<FExtensibilityManager>& Manager)
{
	Construct(Manager, Editor.GetMenuExtensibilityManager().ToSharedRef());
}

void EXPORT_CALL_CONV GetModeBarExtensibilityManager(FLevelEditorModule& Editor, TSharedRef<FExtensibilityManager>& Manager)
{
	Construct(Manager, Editor.GetModeBarExtensibilityManager().ToSharedRef());
}

void EXPORT_CALL_CONV GetNotificationBarExtensibilityManager(FLevelEditorModule& Editor, TSharedRef<FExtensibilityManager>& Manager)
{
	Construct(Manager, Editor.GetNotificationBarExtensibilityManager().ToSharedRef());
}

void EXPORT_CALL_CONV GetToolBarExtensibilityManager(FLevelEditorModule& Editor, TSharedRef<FExtensibilityManager>& Manager)
{
	Construct(Manager, Editor.GetToolBarExtensibilityManager().ToSharedRef());
}

void EXPORT_CALL_CONV RegenerateMenu(FLevelEditorModule& Editor)
{
	TSharedRef<ILevelEditor> EditorWidget{ Editor.GetLevelEditorInstance().Pin().ToSharedRef() };

	SWidget& Child1{ EditorWidget->GetChildren()->GetChildAt(0).Get() };
	check(Child1.GetType() == "SVerticalBox");

	SWidget& Child2{ Child1.GetChildren()->GetChildAt(0).Get() };
	check(Child2.GetType() == "SOverlay");

	SWidget& Child3{ Child2.GetChildren()->GetChildAt(0).Get() };
	check(Child3.GetType() == "SBox");

	check(Child3.GetMetaData<FTagMetaData>()->Tag == "MainMenu");

	static_cast<SBox&>(Child3).SetContent(
		FModuleManager::Get().LoadModuleChecked<IMainFrameModule>("MainFrame").MakeMainTabMenu(
			Editor.GetLevelEditorTabManager(), Editor.GetMenuExtensibilityManager()->GetAllExtenders().ToSharedRef()));
}

const FMemberSymbol Members[] =
{
	EXPORT_METHOD(GetMenuExtensibilityManager),
	EXPORT_METHOD(GetModeBarExtensibilityManager),
	EXPORT_METHOD(GetNotificationBarExtensibilityManager),
	EXPORT_METHOD(GetToolBarExtensibilityManager),
	EXPORT_METHOD(RegenerateMenu),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;

#endif
