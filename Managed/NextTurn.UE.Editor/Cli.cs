// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics;

namespace NextTurn.UE.Editor
{
    internal static class Cli
    {
        internal static List<string> GetSdks()
        {
            var result = new List<string>();

            using var cli = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = "--list-sdks",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                },
            };

            _ = cli.Start();

            string? line;
            while ((line = cli.StandardOutput.ReadLine()) is not null)
            {
                // version [directory]
                result.Add(line.Split(' ')[0]);
            }

            return result;
        }
    }
}
