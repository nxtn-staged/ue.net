// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#pragma once

#if UE_EDITOR

#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/Slate/Public/Framework/Commands/UICommandList.h"

ENABLE_WARNINGS;

namespace NextTurnRuntimeMenu
{
	void MakeMenu(const TSharedRef<FUICommandList>& CommandList);
}

RESTORE_WARNINGS;

#endif
