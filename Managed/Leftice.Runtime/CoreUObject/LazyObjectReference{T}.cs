// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Unreal
{
    public readonly struct LazyObjectReference<T> : IObjectReference<T>, IEquatable<LazyObjectReference<T>>
        where T : Object
    {
        private readonly LazyObjectReference reference;

        public LazyObjectReference(T? target) => this.reference = new LazyObjectReference(target);

        public bool IsValid => this.reference.IsValid;

        public T? Target => this.reference.Target as T;

        Object? IObjectReference.Target => this.reference.Target;

        public override bool Equals(object? value) => value is LazyObjectReference<T> other && this.Equals(other);

        public bool Equals(LazyObjectReference<T> other) => this.reference.Equals(other.reference);

        public override int GetHashCode() => this.reference.GetHashCode();

        public bool TryGetTarget([NotNullWhen(true)] out T? target) => (target = this.Target) != null;

        public bool TryGetTarget([NotNullWhen(true)] out Object? target) => this.TryGetTarget(out target);

        public static bool operator ==(LazyObjectReference<T> left, LazyObjectReference<T> right) => left.Equals(right);

        public static bool operator !=(LazyObjectReference<T> left, LazyObjectReference<T> right) => !(left == right);
    }
}
