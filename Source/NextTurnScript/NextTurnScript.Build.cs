// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

namespace UnrealBuildTool.Rules
{
	public class NextTurnScript : ModuleRules
	{
		public NextTurnScript(ReadOnlyTargetRules Target) : base(Target)
		{
			PrivatePCHHeaderFile = "Private/NextTurnScriptPrivatePCH.hpp";

			PublicDependencyModuleNames.Add(
				"Core"
			);

			PrivateDependencyModuleNames.AddRange(
				new[]
				{
                    "CoreUObject",
                    "NextTurnRuntime",
				}
			);
		}
	}
}
