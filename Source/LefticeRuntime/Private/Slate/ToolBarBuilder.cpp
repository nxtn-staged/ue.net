// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

#if WITH_APPLICATION_CORE

#include "LefticeRuntime/Private/Exports.hpp"

#include "LefticeRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include "Runtime/Slate/Public/Framework/MultiBox/MultiBoxBuilder.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME ToolBarBuilder

TYPE_EXPORT_START

void EXPORT_CALL_CONV AddSeparator(FToolBarBuilder& Builder, FName ExtensionPoint)
{
	Builder.AddSeparator(ExtensionPoint);
}

void EXPORT_CALL_CONV BeginSection(FToolBarBuilder& Builder, FName ExtensionPoint)
{
	Builder.BeginSection(ExtensionPoint);
}

void EXPORT_CALL_CONV EndSection(FToolBarBuilder& Builder)
{
	Builder.EndSection();
}

const FMemberSymbol Members[] =
{
	EXPORT_METHOD(AddSeparator),
	EXPORT_METHOD(BeginSection),
	EXPORT_METHOD(EndSection),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;

#endif
