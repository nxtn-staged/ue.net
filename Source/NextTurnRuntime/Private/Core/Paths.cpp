// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Private/MarshaledTypes.hpp"
#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"
#include "Runtime/Core/Public/Misc/Paths.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME Paths

TYPE_EXPORT_START

void EXPORT_CALL_CONV GetEngineDirectory(FMarshaledString& OutResult)
{
	OutResult = FPaths::EngineDir();
}

void EXPORT_CALL_CONV GetEngineConfigDirectory(FMarshaledString& OutResult)
{
	OutResult = FPaths::EngineConfigDir();
}

void EXPORT_CALL_CONV GetEngineContentDirectory(FMarshaledString& OutResult)
{
	OutResult = FPaths::EngineContentDir();
}

void EXPORT_CALL_CONV GetEnginePluginsDirectory(FMarshaledString& OutResult)
{
	OutResult = FPaths::EnginePluginsDir();
}

void EXPORT_CALL_CONV GetEngineSavedDirectory(FMarshaledString& OutResult)
{
	OutResult = FPaths::EngineSavedDir();
}

void EXPORT_CALL_CONV GetProjectDirectory(FMarshaledString& OutResult)
{
	OutResult = FPaths::ProjectDir();
}

void EXPORT_CALL_CONV GetProjectConfigDirectory(FMarshaledString& OutResult)
{
	OutResult = FPaths::ProjectConfigDir();
}

void EXPORT_CALL_CONV GetProjectContentDirectory(FMarshaledString& OutResult)
{
	OutResult = FPaths::ProjectContentDir();
}

void EXPORT_CALL_CONV GetProjectPluginsDirectory(FMarshaledString& OutResult)
{
	OutResult = FPaths::ProjectPluginsDir();
}

void EXPORT_CALL_CONV GetProjectSavedDirectory(FMarshaledString& OutResult)
{
	OutResult = FPaths::ProjectSavedDir();
}

const FMemberSymbol Members[] =
{
	EXPORT_METHOD(GetEngineDirectory),
	EXPORT_METHOD(GetEngineConfigDirectory),
	EXPORT_METHOD(GetEngineContentDirectory),
	EXPORT_METHOD(GetEnginePluginsDirectory),
	EXPORT_METHOD(GetEngineSavedDirectory),
	EXPORT_METHOD(GetProjectDirectory),
	EXPORT_METHOD(GetProjectConfigDirectory),
	EXPORT_METHOD(GetProjectContentDirectory),
	EXPORT_METHOD(GetProjectPluginsDirectory),
	EXPORT_METHOD(GetProjectSavedDirectory),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
