// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

#pragma once

#include "LefticeRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"
#include "Runtime/Core/Public/Modules/ModuleManager.h"

ENABLE_WARNINGS;

class LEFTICESCRIPT_API ILefticeScript : public IModuleInterface
{
public:
	static const FName LefticeScriptModuleName;

	static ILefticeScript& Get()
	{
		return FModuleManager::LoadModuleChecked<ILefticeScript>(LefticeScriptModuleName);
	}

	static bool IsAvailable()
	{
		return FModuleManager::Get().IsModuleLoaded(LefticeScriptModuleName);
	}
};

RESTORE_WARNINGS;
