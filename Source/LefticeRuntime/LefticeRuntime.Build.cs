// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

namespace UnrealBuildTool.Rules
{
	public class LefticeRuntime : ModuleRules
	{
		public LefticeRuntime(ReadOnlyTargetRules Target) : base(Target)
		{
			PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;

			PrivatePCHHeaderFile = "Private/LefticeRuntimePrivatePCH.hpp";

			PublicDependencyModuleNames.Add(
				"Core"
			);

			PrivateDependencyModuleNames.AddRange(
				new[]
				{
					"CoreUObject",
				}
			);

			if (Target.bCompileAgainstApplicationCore)
			{
				PrivateDependencyModuleNames.AddRange(
					new[]
					{
						"Slate",
						"SlateCore",
					}
				);
			}

			if (Target.bBuildEditor)
			{
				PrivateDependencyModuleNames.AddRange(
					new[]
					{
						"EditorStyle",
						"LevelEditor",
						"MainFrame",
						"UnrealEd",
					}
				);
			}

		}
	}
}
