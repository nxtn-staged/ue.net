// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

using System.Diagnostics.CodeAnalysis;

namespace Unreal
{
    public interface IObjectReference
    {
        bool IsValid { get; }

        Object? Target { get; }

        bool TryGetTarget([NotNullWhen(true)] out Object? target);
    }
}
