// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#pragma once

#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"
#include "Runtime/Core/Public/Modules/ModuleManager.h"

ENABLE_WARNINGS;

class NEXTTURNSCRIPT_API INextTurnScript : public IModuleInterface
{
public:
	static const FName NextTurnScriptModuleName;

	static INextTurnScript& Get()
	{
		return FModuleManager::LoadModuleChecked<INextTurnScript>(NextTurnScriptModuleName);
	}

	static bool IsAvailable()
	{
		return FModuleManager::Get().IsModuleLoaded(NextTurnScriptModuleName);
	}
};

RESTORE_WARNINGS;
