// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Tools.DotNETCommon;

namespace UnrealBuildTool.Rules
{
	public class NextTurnRuntime : ModuleRules
	{
		public NextTurnRuntime(ReadOnlyTargetRules Target) : base(Target)
		{
			PrivatePCHHeaderFile = "Private/NextTurnRuntimePrivatePCH.hpp";

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
						"ToolMenus",
						"UnrealEd",
					}
				);
			}

			using (var Process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = @"C:\Program Files\dotnet\dotnet.exe",
					Arguments = string.Format("restore -r win-x64 \"{0}\"", Path.Combine(PluginDirectory, "Managed", "NextTurn.UE.PreBuild")),
					UseShellExecute = false,
					RedirectStandardOutput = true,
				}
			})
			{
				Process.Start();

				string Line;
				while ((Line = Process.StandardOutput.ReadLine()) != null)
				{
					Log.TraceInformation(Line);

					const string Delimiter = " : ";
					if (Line.Contains(Delimiter))
					{
						string[] Pair = Line.Split(new[] { Delimiter }, 2, StringSplitOptions.None);
						string Key = Pair[0].Trim();
						string Value = Pair[1];
						switch (Key)
						{
							case ".NET Host directory":
								PrivateIncludePaths.Add(Value);
								break;

							case ".NET Host link-time file":
								PublicAdditionalLibraries.Add(Value);
								break;
						}
					}
				}
			}
		}
	}
}
