// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

namespace UnrealBuildTool.Rules
{
	public class LefticeScriptGenerator : ModuleRules
	{
		public LefticeScriptGenerator(ReadOnlyTargetRules Target) : base(Target)
		{
			PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;

			PrivatePCHHeaderFile = "Private/LefticeScriptGeneratorPrivatePCH.hpp";

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
                    "LefticeRuntime",
				}
			);
		}
	}
}
