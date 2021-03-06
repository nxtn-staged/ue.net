// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#if WITH_APPLICATION_CORE

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Private/Construct.hpp"
#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/Slate/Public/Framework/Commands/UICommandInfo.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME BindingContext

TYPE_EXPORT_START

void EXPORT_CALL_CONV Initialize(TSharedRef<FBindingContext>& Context, FName Name, const FText& Description, FName Parent, FName StyleSet)
{
	Construct(Context, TSharedRef<FBindingContext>(new FBindingContext{ Name, Description, Parent, StyleSet }));
}

const FMemberSymbol Members[] =
{
	EXPORT_METHOD(Initialize),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;

#endif
