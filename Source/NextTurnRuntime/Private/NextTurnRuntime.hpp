// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#pragma once

#include "NextTurnRuntime/Public/Compiler.hpp"
#include "NextTurnRuntime/Public/INextTurnRuntime.hpp"

ENABLE_WARNINGS;

RESTORE_WARNINGS;
DECLARE_LOG_CATEGORY_EXTERN(LogNextTurnRuntime, Log, All);
ENABLE_WARNINGS;

class FNextTurnRuntime final : public INextTurnRuntime
{
public:
	// IModuleInterface

	virtual void StartupModule() override;
	virtual void ShutdownModule() override;

	// INextTurnRuntime

	virtual void* GetFunctionPointer(
		const char16_t* AssemblyName,
		int32_t AssemblyNameLength,
		const char16_t* TypeName,
		int32_t TypeNameLength,
		const char16_t* MethodName,
		int32_t MethodNameLength) const override;

	virtual void RegisterSymbols(const FTypeSymbol* Types, int32 TypesLength) const override;
	virtual void UnregisterSymbols(const FTypeSymbol* Types, int32 TypesLength) const override;
};

RESTORE_WARNINGS;
