// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Unreal
{
    public readonly struct WeakObjectReference<T> : IObjectReference<T>, IEquatable<WeakObjectReference<T>>
        where T : Object
    {
        private readonly WeakObjectReference reference;

        public WeakObjectReference(T? target) => this.reference = new WeakObjectReference(target);

        public bool IsValid => this.reference.IsValid;

        public T? Target => this.reference.Target as T;

        Object? IObjectReference.Target => this.reference.Target;

        public override bool Equals(object? value) => value is WeakObjectReference<T> other && this.Equals(other);

        public bool Equals(WeakObjectReference<T> other) => this.reference.Equals(other.reference);

        public override int GetHashCode() => this.reference.GetHashCode();

        public bool TryGetTarget([NotNullWhen(true)] out T? target) => (target = this.Target) != null;

        public bool TryGetTarget([NotNullWhen(true)] out Object? target) => this.TryGetTarget(out target);

        public static bool operator ==(WeakObjectReference<T> left, WeakObjectReference<T> right) => left.Equals(right);

        public static bool operator !=(WeakObjectReference<T> left, WeakObjectReference<T> right) => !(left == right);
    }
}
