// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnRuntime.hpp"

#include "CoreClr.hpp"
#include "NextTurnRuntimeCallback.hpp"
#if UE_EDITOR
#include "NextTurnRuntimeCommands.hpp"
#include "NextTurnRuntimeMenu.hpp"
#endif

#include "Runtime/Core/Public/CoreMinimal.h"
#include "Runtime/Core/Public/Modules/ModuleManager.h"

ENABLE_WARNINGS;

const FName INextTurnRuntime::NextTurnRuntimeModuleName{ "NextTurnRuntime" };

DEFINE_LOG_CATEGORY(LogNextTurnRuntime);

RESTORE_WARNINGS;
IMPLEMENT_MODULE(FNextTurnRuntime, NextTurnRuntime);
ENABLE_WARNINGS;

void FNextTurnRuntime::StartupModule()
{
	CoreClr::Initialize();

#if UE_EDITOR
	FNextTurnRuntimeCommands::Register();

	TSharedRef<FUICommandList> CommandList{ new FUICommandList{} };

	const FNextTurnRuntimeCommands& Commands = FNextTurnRuntimeCommands::Get();

	CommandList->MapAction(
		Commands.Load,
		FExecuteAction::CreateStatic(NextTurnRuntimeCallbacks::Load),
		FCanExecuteAction::CreateStatic(NextTurnRuntimeCallbacks::CanLoad));

	CommandList->MapAction(
		Commands.Reload,
		FExecuteAction::CreateStatic(NextTurnRuntimeCallbacks::Reload),
		FCanExecuteAction::CreateStatic(NextTurnRuntimeCallbacks::CanUnload));

	CommandList->MapAction(
		Commands.Unload,
		FExecuteAction::CreateStatic(NextTurnRuntimeCallbacks::Unload),
		FCanExecuteAction::CreateStatic(NextTurnRuntimeCallbacks::CanUnload));

	NextTurnRuntimeMenu::MakeMenu(CommandList);
#else
	CoreClr::Load();
#endif
}

void FNextTurnRuntime::ShutdownModule()
{
#if UE_EDITOR
	FNextTurnRuntimeCommands::Unregister();
#else
	CoreClr::Unload();
#endif
}

void* FNextTurnRuntime::GetFunctionPointer(
	const char16_t* AssemblyName,
	int32_t AssemblyNameLength,
	const char16_t* TypeName,
	int32_t TypeNameLength,
	const char16_t* MethodName,
	int32_t MethodNameLength) const
{
	return CoreClr::GetFunctionPointer<void*>(
		AssemblyName,
		AssemblyNameLength,
		TypeName,
		TypeNameLength,
		MethodName,
		MethodNameLength);
}

void FNextTurnRuntime::RegisterSymbols(const FTypeSymbol* Types, int32 TypesLength) const
{
	CoreClr::RegisterSymbols(Types, TypesLength);
}

void FNextTurnRuntime::UnregisterSymbols(const FTypeSymbol* Types, int32 TypesLength) const
{
	CoreClr::UnregisterSymbols(Types, TypesLength);
}

RESTORE_WARNINGS;
