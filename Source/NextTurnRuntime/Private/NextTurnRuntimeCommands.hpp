// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#pragma once

#if UE_EDITOR

#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/Slate/Public/Framework/Commands/Commands.h"
#include "Runtime/Slate/Public/Framework/Commands/UICommandInfo.h"

ENABLE_WARNINGS;

class FNextTurnRuntimeCommands final : public TCommands<FNextTurnRuntimeCommands>
{
public:
	FNextTurnRuntimeCommands();

	virtual void RegisterCommands() override;

	TSharedPtr<FUICommandInfo> Load;
	TSharedPtr<FUICommandInfo> Reload;
	TSharedPtr<FUICommandInfo> Unload;
};

RESTORE_WARNINGS;

#endif
