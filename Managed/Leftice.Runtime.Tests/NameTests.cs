// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

using Xunit;

namespace Unreal.Tests
{
    public static class NameTests
    {
        [Fact]
        public static void Constructor()
        {
            Assert.Equal(Name.None, new Name(null));
            Assert.Equal(Name.None, new Name(string.Empty));
        }

        [Fact]
        public static void IsNone()
        {
            Assert.True(Name.None.IsNone);
        }
    }
}
