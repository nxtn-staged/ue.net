// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnScript.hpp"

//#include "NextTurnScript/Private/Exports.hpp"
#include "NextTurnRuntime/Public/INextTurnRuntime.hpp"

ENABLE_WARNINGS;

const FName INextTurnScript::NextTurnScriptModuleName{ "NextTurnScript" };

RESTORE_WARNINGS;
IMPLEMENT_MODULE(FNextTurnScript, NextTurnScript);
ENABLE_WARNINGS;

RESTORE_WARNINGS;

void FNextTurnScript::StartupModule()
{
	//INextTurnRuntime::Get().RegisterSymbols(GetTypeExports(), GetTypeExportsLength());
}

void FNextTurnScript::ShutdownModule()
{
	//INextTurnRuntime::Get().UnregisterSymbols(GetTypeExports(), GetTypeExportsLength());
}
