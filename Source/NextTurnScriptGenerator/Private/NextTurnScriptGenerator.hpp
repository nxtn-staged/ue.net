// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#pragma once

#include "NextTurnScriptGenerator/Public/INextTurnScriptGenerator.hpp"

#include "NextTurnRuntime/Public/Compiler.hpp"

ENABLE_WARNINGS;

RESTORE_WARNINGS;
DECLARE_LOG_CATEGORY_EXTERN(LogNextTurnScriptGenerator, Log, All);
ENABLE_WARNINGS;

class FNextTurnScriptGenerator final : public INextTurnScriptGenerator
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
