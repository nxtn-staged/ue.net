// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

#pragma warning disable SA1604 // Element documentation should have summary
#pragma warning disable SA1627 // Documentation text should not be empty

using System;
using System.Diagnostics.CodeAnalysis;

namespace Unreal
{
    internal static class Throw
    {
        /// <exception cref="ArgumentNullException"/>
        [DoesNotReturn]
        internal static void ArrayArgumentNullException() => throw new ArgumentNullException("array");

        /// <exception cref="ArgumentNullException"/>
        [DoesNotReturn]
        internal static void HelpMessageArgumentNullException() => throw new ArgumentNullException("helpMessage");

        /// <exception cref="ArgumentOutOfRangeException"/>
        [DoesNotReturn]
        internal static void IndexArgumentOutOfRangeException() => throw new ArgumentOutOfRangeException("index");

        /// <exception cref="System.InvalidOperationException"/>
        [DoesNotReturn]
        internal static void InvalidOperationException() => throw new InvalidOperationException();

        /// <exception cref="ArgumentNullException"/>
        [DoesNotReturn]
        internal static void NameArgumentNullException() => throw new ArgumentNullException("name");

        /// <exception cref="System.NotSupportedException"/>
        [DoesNotReturn]
        internal static void NotSupportedException() => throw new NotSupportedException();
    }
}
