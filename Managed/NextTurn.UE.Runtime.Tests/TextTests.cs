// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using Xunit;

namespace Unreal.Tests
{
    public static class TextTests
    {
        [Fact]
        public static void IsNullOrEmpty()
        {
            Assert.True(Text.IsNullOrEmpty(null));
        }

        [Fact]
        public static void IsNullOrWhiteSpace()
        {
            Assert.True(Text.IsNullOrWhiteSpace(null));
        }
    }
}
