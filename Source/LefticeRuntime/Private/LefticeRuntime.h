// Copyright (c) NextTurn.
// See the LICENSE file in the project root for more information.

#pragma once

#include "ILefticeRuntime.h"

class FLefticeRuntime : public ILefticeRuntime
{
public:
	virtual void StartupModule() override;
	virtual void ShutdownModule() override;
};
