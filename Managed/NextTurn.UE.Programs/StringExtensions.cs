// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;

namespace NextTurn.UE.Programs
{
    internal static class StringExtensions
    {
        internal static bool StartsWith(this string str, string value1, string value2) =>
            str.StartsWith(value1) && str.AsSpan()[value1.Length..].StartsWith(value2);
    }
}
