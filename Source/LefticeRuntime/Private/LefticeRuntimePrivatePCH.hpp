// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

#pragma once

#include "Runtime/Core/Public/CoreMinimal.h"
#include "Runtime/Core/Public/HAL/ConsoleManager.h"
#include "Runtime/Core/Public/HAL/PlatformProcess.h"
#include "Runtime/Core/Public/Misc/MessageDialog.h"
#include "Runtime/Core/Public/Misc/Paths.h"
#include "Runtime/Core/Public/Modules/ModuleManager.h"

#include "Runtime/CoreUObject/Public/UObject/Class.h"
#include "Runtime/CoreUObject/Public/UObject/EnumProperty.h"
#include "Runtime/CoreUObject/Public/UObject/MetaData.h"
#include "Runtime/CoreUObject/Public/UObject/Package.h"
#include "Runtime/CoreUObject/Public/UObject/TextProperty.h"
#include "Runtime/CoreUObject/Public/UObject/UObjectHash.h"
#include "Runtime/CoreUObject/Public/UObject/UnrealType.h"
#include "Runtime/CoreUObject/Public/UObject/WeakObjectPtr.h"

#if WITH_APPLICATION_CORE
#include "Runtime/Slate/Public/Framework/Commands/Commands.h"
#include "Runtime/Slate/Public/Framework/Commands/UICommandInfo.h"
#include "Runtime/Slate/Public/Framework/Commands/UICommandList.h"
#include "Runtime/Slate/Public/Framework/MultiBox/MultiBoxBuilder.h"
#include "Runtime/Slate/Public/Framework/MultiBox/MultiBoxExtender.h"
#include "Runtime/Slate/Public/Widgets/Layout/SBorder.h"
#include "Runtime/Slate/Public/Widgets/Layout/SBox.h"
#include "Runtime/Slate/Public/Widgets/Layout/SGridPanel.h"

#include "Runtime/SlateCore/Public/SlotBase.h"
#include "Runtime/SlateCore/Public/Widgets/SBoxPanel.h"
#include "Runtime/SlateCore/Public/Widgets/SWidget.h"
#endif

#if WITH_EDITOR
#include "Editor/LevelEditor/Private/SLevelEditor.h"
#include "Editor/LevelEditor/Public/ILevelEditor.h"
#include "Editor/LevelEditor/Public/LevelEditor.h"

#include "Editor/MainFrame/Public/Interfaces/IMainFrameModule.h"

#include "Editor/UnrealEd/Public/Toolkits/AssetEditorToolkit.h"
#endif
