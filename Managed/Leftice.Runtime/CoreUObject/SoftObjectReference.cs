// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using Leftice.Processors;

namespace Unreal
{
    public readonly struct SoftObjectReference : IObjectReference, IEquatable<SoftObjectReference>
    {
        private readonly PersistentObjectReference<SoftObjectPath> reference;

        public SoftObjectReference(Object? target) => NativeMethods.Initialize(out this, Object.GetPointerOrZero(target));

        public bool IsValid => this.reference.IsValid;

        public Object? Target => this.reference.Target;

        public override bool Equals(object? value) => value is SoftObjectReference other && this.Equals(other);

        public bool Equals(SoftObjectReference other) => this.reference.Equals(other.reference);

        public override int GetHashCode() => this.reference.GetHashCode();

        public bool TryGetTarget([NotNullWhen(true)] out Object? target) => this.reference.TryGetTarget(out target);

        public static bool operator ==(SoftObjectReference left, SoftObjectReference right) => left.Equals(right);

        public static bool operator !=(SoftObjectReference left, SoftObjectReference right) => !(left == right);

        private static class NativeMethods
        {
            [Calli]
            public static extern void Initialize(out SoftObjectReference reference, IntPtr target);
        }
    }
}
