// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Programs/UnrealHeaderTool/Public/IScriptGeneratorPluginInterface.h"

#include <type_traits>

ENABLE_WARNINGS;

// NOTE: Keep in sync with NextTurn.UE.Programs.ModuleType.
enum class ENextTurnModuleType
{
	Program,
	EngineRuntime,
	EngineUncooked,
	EngineDeveloper,
	EngineEditor,
	EngineThirdParty,
	ProjectRuntime,
	ProjectUncooked,
	ProjectDeveloper,
	ProjectEditor,
	ProjectThirdParty,

	Max,
};

#ifndef UNDERLYING_VALUE
#define UNDERLYING_VALUE(E) static_cast<std::underlying_type_t<decltype(E)>>(E)
#endif

static_assert(sizeof(ENextTurnModuleType) == sizeof(EBuildModuleType::Type), "");

static_assert(UNDERLYING_VALUE(ENextTurnModuleType::Program) == EBuildModuleType::Program, "");
static_assert(UNDERLYING_VALUE(ENextTurnModuleType::EngineRuntime) == EBuildModuleType::EngineRuntime, "");
static_assert(UNDERLYING_VALUE(ENextTurnModuleType::EngineDeveloper) == EBuildModuleType::EngineDeveloper, "");
static_assert(UNDERLYING_VALUE(ENextTurnModuleType::EngineEditor) == EBuildModuleType::EngineEditor, "");
static_assert(UNDERLYING_VALUE(ENextTurnModuleType::EngineThirdParty) == EBuildModuleType::EngineThirdParty, "");
static_assert(UNDERLYING_VALUE(ENextTurnModuleType::ProjectRuntime) == EBuildModuleType::GameRuntime, "");
static_assert(UNDERLYING_VALUE(ENextTurnModuleType::ProjectDeveloper) == EBuildModuleType::GameDeveloper, "");
static_assert(UNDERLYING_VALUE(ENextTurnModuleType::ProjectEditor) == EBuildModuleType::GameEditor, "");
static_assert(UNDERLYING_VALUE(ENextTurnModuleType::ProjectThirdParty) == EBuildModuleType::GameThirdParty, "");
static_assert(UNDERLYING_VALUE(ENextTurnModuleType::Max) == EBuildModuleType::Max, "");

RESTORE_WARNINGS;
