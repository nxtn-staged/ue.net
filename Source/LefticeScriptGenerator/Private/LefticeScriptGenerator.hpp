// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

#pragma once

#include "LefticeScriptGenerator/Public/ILefticeScriptGenerator.hpp"

#include "LefticeRuntime/Public/Compiler.hpp"

ENABLE_WARNINGS;

DECLARE_LOG_CATEGORY_EXTERN(LogLefticeScriptGenerator, Log, All);

class FLefticeScriptGenerator final : public ILefticeScriptGenerator
{
public:
	// IModuleInterface

	virtual void StartupModule() override;
	virtual void ShutdownModule() override;

	// IScriptGeneratorPluginInterface

	virtual FString GetGeneratedCodeModuleName() const override;
	virtual bool SupportsTarget(const FString& TargetName) const override;
	virtual bool ShouldExportClassesForModule(const FString& ModuleName, EBuildModuleType::Type ModuleType, const FString& /*ModuleGeneratedIncludeDirectory*/) const override;
	virtual void Initialize(const FString& /*RootLocalPath*/, const FString& /*RootBuildPath*/, const FString& OutputDirectory, const FString& IncludeBase) override;
	virtual void ExportClass(UClass* Class, const FString& /*SourceHeaderFilename*/, const FString& /*GeneratedHeaderFilename*/, bool /*bHasChanged*/) override;
	virtual void FinishExport() override;
	virtual FString GetGeneratorName() const override;
};

RESTORE_WARNINGS;
