// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnRuntimeCallback.hpp"

#include "CoreClr.hpp"

#include "INextTurnRuntime.hpp"

ENABLE_WARNINGS;

namespace NextTurnRuntimeCallbacks
{
// #if UE_EDITOR
	constexpr char16_t EntryPointAssembyName[] = u"NextTurn.UE.Editor";
	constexpr char16_t EntryPointTypeName[] = u"NextTurn.UE.Editor.EntryPoint";
// #endif

	bool Loaded;

	bool CanLoad()
	{
		return true;
	}

	bool CanUnload()
	{
		return true;
	}

	void Load()
	{
		CoreClr::Load();

		void (STDCALL *Start_Method)() = INextTurnRuntime::Get().GetFunctionPointer<decltype(Start_Method)>(
			EntryPointAssembyName, EntryPointTypeName, u"Start");

		Start_Method();

		Loaded = true;
	}

	void Reload()
	{
		Unload();
		Load();
	}

	void Unload()
	{
		if (Loaded)
		{
			void (STDCALL * Stop_Method)() = INextTurnRuntime::Get().GetFunctionPointer<decltype(Stop_Method)>(
				EntryPointAssembyName, EntryPointTypeName, u"Stop");

			Stop_Method();

			Loaded = false;
		}

		CoreClr::Unload();
	}
}

RESTORE_WARNINGS;
