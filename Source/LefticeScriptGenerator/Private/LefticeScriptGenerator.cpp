// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

#include "LefticeScriptGenerator.hpp"

#include "LefticeRuntime/Public/ILefticeRuntime.hpp"

#include "Runtime/Core/Public/Features/IModularFeatures.h"

ENABLE_WARNINGS;

const FName ILefticeScriptGenerator::LefticeScriptGeneratorModuleName{ "LefticeScriptGenerator" };

DEFINE_LOG_CATEGORY(LogLefticeScriptGenerator);

RESTORE_WARNINGS;
IMPLEMENT_MODULE(FLefticeScriptGenerator, LefticeScriptGenerator)
ENABLE_WARNINGS;

namespace
{
	const FName ScriptGeneratorFeatureName{ "ScriptGenerator" };

	void (__stdcall *Initialize_Method)();
	void (__stdcall *AddClass_Method)(const UClass* Class);
	void (__stdcall *FinishExport_Method)();
}

void FLefticeScriptGenerator::StartupModule()
{
	IModularFeatures::Get().RegisterModularFeature(ScriptGeneratorFeatureName, this);
}

void FLefticeScriptGenerator::ShutdownModule()
{
	IModularFeatures::Get().UnregisterModularFeature(ScriptGeneratorFeatureName, this);
}

FString FLefticeScriptGenerator::GetGeneratedCodeModuleName() const
{
	return TEXT("LefticeScript");
}

bool FLefticeScriptGenerator::SupportsTarget(const FString& TargetName) const
{
	RESTORE_WARNINGS;
	UE_LOG(LogLefticeScriptGenerator, Display, TEXT("Supports %s"), *TargetName);
	ENABLE_WARNINGS;
	return true;
}

bool FLefticeScriptGenerator::ShouldExportClassesForModule(const FString& ModuleName, EBuildModuleType::Type ModuleType, const FString& /*ModuleGeneratedIncludeDirectory*/) const
{
	RESTORE_WARNINGS;
	UE_LOG(LogLefticeScriptGenerator, Display, TEXT("%s : %u"), *ModuleName, ModuleType);
	ENABLE_WARNINGS;
	return ModuleType == EBuildModuleType::EngineRuntime || ModuleType == EBuildModuleType::GameRuntime;
}

void FLefticeScriptGenerator::Initialize(const FString& /*RootLocalPath*/, const FString& /*RootBuildPath*/, const FString& OutputDirectory, const FString& IncludeBase)
{
	RESTORE_WARNINGS;
	UE_LOG(LogLefticeScriptGenerator, Display, TEXT("Output directory: %s"), *OutputDirectory);
	UE_LOG(LogLefticeScriptGenerator, Display, TEXT("Include base: %s"), *IncludeBase);
	ENABLE_WARNINGS;

	constexpr char16_t AssemblyGeneratorAssemblyName[] = u"Leftice.Programs";
	constexpr char16_t AssemblyGeneratorTypeName[] = u"Leftice.Programs.AssemblyGenerator";

	ILefticeRuntime& Runtime{ ILefticeRuntime::Get() };

	Initialize_Method = Runtime.GET_FUNCTION_POINTER(
		decltype(Initialize_Method), AssemblyGeneratorAssemblyName, AssemblyGeneratorTypeName, u"Initialize");

	AddClass_Method = Runtime.GET_FUNCTION_POINTER(
		decltype(AddClass_Method), AssemblyGeneratorAssemblyName, AssemblyGeneratorTypeName, u"AddClass");

	FinishExport_Method = Runtime.GET_FUNCTION_POINTER(
		decltype(FinishExport_Method), AssemblyGeneratorAssemblyName, AssemblyGeneratorTypeName, u"FinishExport");

	Initialize_Method();
}

void FLefticeScriptGenerator::ExportClass(UClass* Class, const FString& /*SourceHeaderFilename*/, const FString& /*GeneratedHeaderFilename*/, bool /*bHasChanged*/)
{
	AddClass_Method(Class);
}

void FLefticeScriptGenerator::FinishExport()
{
	FinishExport_Method();
}

FString FLefticeScriptGenerator::GetGeneratorName() const
{
	return TEXT("Leftice Script Generator");
}

RESTORE_WARNINGS;
