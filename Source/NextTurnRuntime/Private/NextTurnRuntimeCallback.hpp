// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#pragma once

#include "NextTurnRuntime/Public/Compiler.hpp"

ENABLE_WARNINGS;

namespace NextTurnRuntimeCallbacks
{
	bool CanLoad();
	bool CanUnload();
	void Load();
	void Reload();
	void Unload();
}

RESTORE_WARNINGS;
