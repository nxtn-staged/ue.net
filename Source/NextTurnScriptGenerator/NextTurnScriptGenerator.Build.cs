// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

namespace UnrealBuildTool.Rules
{
	public class NextTurnScriptGenerator : ModuleRules
	{
		public NextTurnScriptGenerator(ReadOnlyTargetRules Target) : base(Target)
		{
			PrivatePCHHeaderFile = "Private/NextTurnScriptGeneratorPrivatePCH.hpp";

			PublicIncludePaths.Add(
				"Programs/UnrealHeaderTool/Public"
			);

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
