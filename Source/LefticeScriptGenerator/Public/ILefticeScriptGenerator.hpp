// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

#pragma once

#include "LefticeRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"
#include "Runtime/Core/Public/Modules/ModuleManager.h"

#include "Programs/UnrealHeaderTool/Public/IScriptGeneratorPluginInterface.h"

ENABLE_WARNINGS;

// For IntelliSense
#ifndef LEFTICESCRIPTGENERATOR_API
#define LEFTICESCRIPTGENERATOR_API
#endif

class LEFTICESCRIPTGENERATOR_API ILefticeScriptGenerator : public IScriptGeneratorPluginInterface
{
public:
	static const FName LefticeScriptGeneratorModuleName;

	static ILefticeScriptGenerator& Get()
	{
		return FModuleManager::LoadModuleChecked<ILefticeScriptGenerator>(LefticeScriptGeneratorModuleName);
	}

	static bool IsAvailable()
	{
		return FModuleManager::Get().IsModuleLoaded(LefticeScriptGeneratorModuleName);
	}
};

RESTORE_WARNINGS;
