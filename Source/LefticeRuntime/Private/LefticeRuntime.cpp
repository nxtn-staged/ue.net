// Copyright (c) NextTurn.
// See the LICENSE file in the project root for more information.

#include "LefticeRuntime.h"

#include <CoreMinimal.h>
#include <Modules/ModuleManager.h>

const FName ILefticeRuntime::LefticeRuntimeModuleName{ "LefticeRuntime" };

IMPLEMENT_MODULE(FLefticeRuntime, LefticeRuntime)

void FLefticeRuntime::StartupModule()
{
}

void FLefticeRuntime::ShutdownModule()
{
}
