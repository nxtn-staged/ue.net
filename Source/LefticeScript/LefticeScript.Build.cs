// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

namespace UnrealBuildTool.Rules
{
	public class LefticeScript : ModuleRules
	{
		public LefticeScript(ReadOnlyTargetRules Target) : base(Target)
		{
			PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;

			PrivatePCHHeaderFile = "Private/LefticeScriptPrivatePCH.hpp";

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
