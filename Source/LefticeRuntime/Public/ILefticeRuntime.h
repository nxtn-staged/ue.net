// Copyright (c) NextTurn.
// See the LICENSE file in the project root for more information.

#pragma once

#include <CoreMinimal.h>
#include <Modules/ModuleManager.h>

class LEFTICERUNTIME_API ILefticeRuntime : public IModuleInterface
{
public:
	static const FName LefticeRuntimeModuleName;

	static ILefticeRuntime& Get()
	{
		return FModuleManager::LoadModuleChecked<ILefticeRuntime>(LefticeRuntimeModuleName);
	}

	static bool IsAvailable()
	{
		return FModuleManager::Get().IsModuleLoaded(LefticeRuntimeModuleName);
	}
};
