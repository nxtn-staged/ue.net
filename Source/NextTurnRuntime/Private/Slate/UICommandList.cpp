// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#if WITH_APPLICATION_CORE

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Private/Construct.hpp"
#include "NextTurnRuntime/Private/ManagedDelegate.hpp"
#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/Slate/Public/Framework/Commands/UICommandList.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME UICommandList

TYPE_EXPORT_START

void EXPORT_CALL_CONV Initialize(TSharedRef<FUICommandList>& CommandList)
{
	Construct(CommandList, TSharedRef<FUICommandList>{ new FUICommandList{} });
}

void EXPORT_CALL_CONV MapAction(
	const TSharedRef<FUICommandList>& CommandList,
	const TSharedRef<const FUICommandInfo>& CommandInfo,
	FExecuteAction::FStaticDelegate::FFuncPtr Execute,
	intptr_t ExecuteHandle)
{
	CommandList->MapAction(CommandInfo, TManagedDelegate<FExecuteAction>::Create(Execute, ExecuteHandle));
}

const FMemberSymbol Members[] =
{
	EXPORT_METHOD(Initialize),
	EXPORT_METHOD(MapAction),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;

#endif
