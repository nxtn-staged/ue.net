// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Unreal
{
    public readonly struct SoftObjectReference<T> : IObjectReference<T>, IEquatable<SoftObjectReference<T>>
        where T : Object
    {
        private readonly SoftObjectReference reference;

        public SoftObjectReference(T? target) => this.reference = new SoftObjectReference(target);

        public bool IsValid => this.reference.IsValid;

        public T? Target => this.reference.Target as T;

        Object? IObjectReference.Target => this.reference.Target;

        public override bool Equals(object? value) => value is WeakObjectReference<T> other && this.Equals(other);

        public bool Equals(SoftObjectReference<T> other) => this.reference.Equals(other.reference);

        public override int GetHashCode() => this.reference.GetHashCode();

        public bool TryGetTarget([NotNullWhen(true)] out T? target) => (target = this.Target) != null;

        public bool TryGetTarget([NotNullWhen(true)] out Object? target) => this.TryGetTarget(out target);

        public static bool operator ==(SoftObjectReference<T> left, SoftObjectReference<T> right) => left.Equals(right);

        public static bool operator !=(SoftObjectReference<T> left, SoftObjectReference<T> right) => !(left == right);
    }
}
