// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

#pragma once

#include "LefticeRuntime/Public/Compiler.hpp"
#include "LefticeRuntime/Public/ILefticeRuntime.hpp"

ENABLE_WARNINGS;

DECLARE_LOG_CATEGORY_EXTERN(LogLefticeRuntime, Log, All);

class FLefticeRuntime final : public ILefticeRuntime
{
public:
	// IModuleInterface

	virtual void StartupModule() override;
	virtual void ShutdownModule() override;

	// ILefticeRuntime

	virtual void* GetFunctionPointer(
		const char16_t* AssemblyName,
		int32_t AssemblyNameLength,
		const char16_t* TypeName,
		int32_t TypeNameLength,
		const char16_t* MethodName,
		int32_t MethodNameLength,
		const char16_t* DelegateTypeName,
		int32_t DelegateTypeNameLength) const override;
};

RESTORE_WARNINGS;
