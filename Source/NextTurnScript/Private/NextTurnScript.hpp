// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#pragma once

#include "NextTurnScript/Public/INextTurnScript.hpp"

#include "NextTurnRuntime/Public/Compiler.hpp"

ENABLE_WARNINGS;

class FNextTurnScript final : public INextTurnScript
{
public:
	// IModuleInterface

	virtual void StartupModule() override;
	virtual void ShutdownModule() override;
};

RESTORE_WARNINGS;
