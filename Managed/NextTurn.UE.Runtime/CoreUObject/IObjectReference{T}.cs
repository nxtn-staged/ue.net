// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System.Diagnostics.CodeAnalysis;

namespace Unreal
{
    public interface IObjectReference<T> : IObjectReference
        where T : Object
    {
        new T? Target { get; }

        bool TryGetTarget([NotNullWhen(true)] out T? target);
    }
}
