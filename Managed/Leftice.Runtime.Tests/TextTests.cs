// Copyright (c) NextTurn.
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
