// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Private/MarshaledTypes.hpp"
#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"
#include "Runtime/Core/Public/Modules/ModuleManager.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME ModuleManager

TYPE_EXPORT_START

IModuleInterface* EXPORT_CALL_CONV GetModule(const FMarshaledName Name)
{
	return FModuleManager::Get().GetModule(Name.ToName());
}

bool EXPORT_CALL_CONV IsModuleLoaded(const FMarshaledName Name)
{
	return FModuleManager::Get().IsModuleLoaded(Name.ToName());
}

IModuleInterface* EXPORT_CALL_CONV LoadModule(const FMarshaledName Name)
{
	return FModuleManager::Get().LoadModule(Name.ToName());
}

const FMemberSymbol Members[] =
{
	EXPORT_METHOD(GetModule),
	EXPORT_METHOD(IsModuleLoaded),
	EXPORT_METHOD(LoadModule),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
