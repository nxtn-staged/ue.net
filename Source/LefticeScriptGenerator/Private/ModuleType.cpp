// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

#include "LefticeRuntime/Public/Compiler.hpp"

#include "Programs/UnrealHeaderTool/Public/IScriptGeneratorPluginInterface.h"

#include <type_traits>

ENABLE_WARNINGS;

enum class ELefticeModuleType
{
	Program,
	EngineRuntime,
	EngineDeveloper,
	EngineEditor,
	EngineThirdParty,
	ProjectRuntime,
	ProjectDeveloper,
	ProjectEditor,
	ProjectThirdParty,

	Max,
};

#define UNDERLYING_VALUE(E) static_cast<std::underlying_type_t<decltype(E)>>(E)

static_assert(UNDERLYING_VALUE(ELefticeModuleType::Program) == EBuildModuleType::Program, "");
static_assert(UNDERLYING_VALUE(ELefticeModuleType::EngineRuntime) == EBuildModuleType::EngineRuntime, "");
static_assert(UNDERLYING_VALUE(ELefticeModuleType::EngineDeveloper) == EBuildModuleType::EngineDeveloper, "");
static_assert(UNDERLYING_VALUE(ELefticeModuleType::EngineEditor) == EBuildModuleType::EngineEditor, "");
static_assert(UNDERLYING_VALUE(ELefticeModuleType::EngineThirdParty) == EBuildModuleType::EngineThirdParty, "");
static_assert(UNDERLYING_VALUE(ELefticeModuleType::ProjectRuntime) == EBuildModuleType::GameRuntime, "");
static_assert(UNDERLYING_VALUE(ELefticeModuleType::ProjectDeveloper) == EBuildModuleType::GameDeveloper, "");
static_assert(UNDERLYING_VALUE(ELefticeModuleType::ProjectEditor) == EBuildModuleType::GameEditor, "");
static_assert(UNDERLYING_VALUE(ELefticeModuleType::ProjectThirdParty) == EBuildModuleType::GameThirdParty, "");
static_assert(UNDERLYING_VALUE(ELefticeModuleType::Max) == EBuildModuleType::Max, "");

RESTORE_WARNINGS;
