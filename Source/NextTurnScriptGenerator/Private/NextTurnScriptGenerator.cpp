// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnScriptGenerator.hpp"

#include "NextTurnRuntime/Public/INextTurnRuntime.hpp"

#include "Runtime/Core/Public/Features/IModularFeatures.h"

ENABLE_WARNINGS;

const FName INextTurnScriptGenerator::NextTurnScriptGeneratorModuleName{ "NextTurnScriptGenerator" };

DEFINE_LOG_CATEGORY(LogNextTurnScriptGenerator);

RESTORE_WARNINGS;
IMPLEMENT_MODULE(FNextTurnScriptGenerator, NextTurnScriptGenerator)
ENABLE_WARNINGS;

namespace
{
	const FName ScriptGeneratorFeatureName{ "ScriptGenerator" };

	void (STDCALL *Initialize_Method)(const FString& OutputDirectory);
	void (STDCALL *AddClass_Method)(const UClass* Class);
	void (STDCALL *FinishExport_Method)();
}

void FNextTurnScriptGenerator::StartupModule()
{
	//IModularFeatures::Get().RegisterModularFeature(ScriptGeneratorFeatureName, this);
}

void FNextTurnScriptGenerator::ShutdownModule()
{
	//IModularFeatures::Get().UnregisterModularFeature(ScriptGeneratorFeatureName, this);
}

FString FNextTurnScriptGenerator::GetGeneratedCodeModuleName() const
{
	return TEXT("NextTurnScript");
}

bool FNextTurnScriptGenerator::SupportsTarget(const FString& TargetName) const
{
	RESTORE_WARNINGS;
	UE_LOG(LogNextTurnScriptGenerator, Display, TEXT("Supports %s"), *TargetName);
	ENABLE_WARNINGS;
	return true;
}

bool FNextTurnScriptGenerator::ShouldExportClassesForModule(const FString& /*ModuleName*/, EBuildModuleType::Type ModuleType, const FString& /*ModuleGeneratedIncludeDirectory*/) const
{
	//RESTORE_WARNINGS;
	//UE_LOG(LogNextTurnScriptGenerator, Display, TEXT("%s : %u"), *ModuleName, ModuleType);
	//ENABLE_WARNINGS;
	return ModuleType == EBuildModuleType::EngineRuntime || ModuleType == EBuildModuleType::GameRuntime;
}

void FNextTurnScriptGenerator::Initialize(const FString& /*RootLocalPath*/, const FString& /*RootBuildPath*/, const FString& OutputDirectory, const FString& IncludeBase)
{
	RESTORE_WARNINGS;
	UE_LOG(LogNextTurnScriptGenerator, Display, TEXT("Output directory: %s"), *OutputDirectory);
	UE_LOG(LogNextTurnScriptGenerator, Display, TEXT("Include base: %s"), *IncludeBase);
	ENABLE_WARNINGS;

	constexpr char16_t AssemblyGeneratorAssemblyName[] = u"NextTurn.UE.Programs";
	constexpr char16_t AssemblyGeneratorTypeName[] = u"NextTurn.UE.Programs.CSharpCodeGenerator";

	INextTurnRuntime& Runtime{ INextTurnRuntime::Get() };

	Initialize_Method = Runtime.GetFunctionPointer<decltype(Initialize_Method)>(
		AssemblyGeneratorAssemblyName, AssemblyGeneratorTypeName, u"Initialize");

	AddClass_Method = Runtime.GetFunctionPointer<decltype(AddClass_Method)>(
		AssemblyGeneratorAssemblyName, AssemblyGeneratorTypeName, u"AddClass");

	FinishExport_Method = Runtime.GetFunctionPointer<decltype(FinishExport_Method)>(
		AssemblyGeneratorAssemblyName, AssemblyGeneratorTypeName, u"FinishExport");

	Initialize_Method(OutputDirectory);
}

void FNextTurnScriptGenerator::ExportClass(UClass* Class, const FString& /*SourceHeaderFilename*/, const FString& /*GeneratedHeaderFilename*/, bool /*bHasChanged*/)
{
	AddClass_Method(Class);
}

void FNextTurnScriptGenerator::FinishExport()
{
	FinishExport_Method();
}

FString FNextTurnScriptGenerator::GetGeneratorName() const
{
	return TEXT("NextTurn Script Generator");
}

RESTORE_WARNINGS;
