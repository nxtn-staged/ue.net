// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Private/Accessor.hpp"
#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"
#include "Runtime/Core/Public/Misc/SlowTask.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME SlowTask

TYPE_EXPORT_START

FSlowTask* EXPORT_CALL_CONV New(float TotalAmount, const FText& DefaultMessage)
{
	FSlowTask* Result = new FSlowTask(TotalAmount, DefaultMessage);
	Result->Initialize();
	return Result;
}

void EXPORT_CALL_CONV Delete(FSlowTask* Task)
{
	Task->Destroy();
	delete Task;
}

const FMemberSymbol Members[] =
{
	EXPORT_OFFSET_PUBLIC(FSlowTask, CompletedAmount, CompletedWork),
	EXPORT_OFFSET_PUBLIC(FSlowTask, DefaultMessage, DefaultMessage),
	EXPORT_OFFSET_PUBLIC(FSlowTask, FrameMessage, FrameMessage),
	EXPORT_OFFSET_PUBLIC(FSlowTask, TotalAmount, TotalAmountOfWork),

	EXPORT_METHOD(New),
	EXPORT_METHOD(Delete),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
