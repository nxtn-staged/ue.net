// Copyright (c) NextTurn.
// See the LICENSE file in the project root for more information.

namespace UnrealBuildTool.Rules
{
	public class LefticeRuntime : ModuleRules
	{
		public LefticeRuntime(ReadOnlyTargetRules Target) : base(Target)
		{
			PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;

			PrivatePCHHeaderFile = "Private/LefticeRuntimePrivatePCH.h";

			PublicDependencyModuleNames.AddRange(
				new[]
				{
					"Core"
				}
			);
		}
	}
}
