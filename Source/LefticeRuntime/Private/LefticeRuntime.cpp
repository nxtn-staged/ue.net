// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

#include "LefticeRuntime.h"

#include "CoreClr.h"

#include <CoreMinimal.h>
#include <Modules/ModuleManager.h>

const FName ILefticeRuntime::LefticeRuntimeModuleName{ "LefticeRuntime" };

DEFINE_LOG_CATEGORY(LogLefticeRuntime);

IMPLEMENT_MODULE(FLefticeRuntime, LefticeRuntime)

void FLefticeRuntime::StartupModule()
{
	CoreClr::Initialize();
}

void FLefticeRuntime::ShutdownModule()
{
}

void* FLefticeRuntime::GetFunctionPointer(const char16_t* TypeName, const char16_t* MethodName) const
{
	return CoreClr::GetFunctionPointer<void*>(TypeName, MethodName);
}
