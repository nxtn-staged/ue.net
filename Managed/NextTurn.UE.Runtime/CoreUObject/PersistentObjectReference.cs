// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Unreal
{
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct PersistentObjectReference<T> : IObjectReference, IEquatable<PersistentObjectReference<T>> where T : unmanaged, IEquatable<T>
    {
        private readonly WeakObjectReference reference;
        private readonly int unused;
        private readonly T id;

        public bool IsValid => throw new NotImplementedException();

        public Object? Target => throw new NotImplementedException();

        public override bool Equals(object? value) => value is PersistentObjectReference<T> other && this.Equals(other);

        public bool Equals(PersistentObjectReference<T> other) => this.id.Equals(other.id);

        public override int GetHashCode() => this.id.GetHashCode();

        public bool TryGetTarget([NotNullWhen(true)] out Object? target) => this.reference.TryGetTarget(out target);

        public static bool operator ==(PersistentObjectReference<T> left, PersistentObjectReference<T> right) => left.Equals(right);

        public static bool operator !=(PersistentObjectReference<T> left, PersistentObjectReference<T> right) => !(left == right);
    }
}
