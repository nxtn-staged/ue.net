// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#pragma once

#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"
#include "Runtime/Core/Public/Modules/ModuleManager.h"

#include "Programs/UnrealHeaderTool/Public/IScriptGeneratorPluginInterface.h"

ENABLE_WARNINGS;

#ifdef __INTELLISENSE__
#define NEXTTURNSCRIPTGENERATOR_API
#endif

class NEXTTURNSCRIPTGENERATOR_API INextTurnScriptGenerator : public IScriptGeneratorPluginInterface
{
public:
	static const FName NextTurnScriptGeneratorModuleName;

	static INextTurnScriptGenerator& Get()
	{
		return FModuleManager::LoadModuleChecked<INextTurnScriptGenerator>(NextTurnScriptGeneratorModuleName);
	}

	static bool IsAvailable()
	{
		return FModuleManager::Get().IsModuleLoaded(NextTurnScriptGeneratorModuleName);
	}
};

RESTORE_WARNINGS;
