// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public readonly struct WeakObjectReference : IObjectReference, IEquatable<WeakObjectReference>
    {
        private readonly int index;
        private readonly int serialNumber;

        public WeakObjectReference(Object? target) => NativeMethods.Initialize(out this, Object.GetPointerOrZero(target));

        public bool IsValid => NativeMethods.IsValid(this);

        public Object? Target => Object.CreateOrNull<Object>(NativeMethods.GetTarget(this));

        public override bool Equals(object? value) => value is WeakObjectReference other && this.Equals(other);

        public bool Equals(WeakObjectReference other) =>
            (this.index == other.index && this.serialNumber == other.serialNumber) ||
            (!this.IsValid && !other.IsValid);

        public override int GetHashCode() => this.index ^ this.serialNumber;

        public bool TryGetTarget([NotNullWhen(true)] out Object? target) => (target = this.Target) != null;

        public static bool operator ==(WeakObjectReference left, WeakObjectReference right) => left.Equals(right);

        public static bool operator !=(WeakObjectReference left, WeakObjectReference right) => !(left == right);

        internal static class NativeMethods
        {
            [Calli]
            public static extern void Initialize(out WeakObjectReference reference, IntPtr target);

            [Calli]
            public static extern IntPtr GetTarget(in WeakObjectReference reference);

            [Calli]
            public static extern bool IsValid(in WeakObjectReference reference);
        }
    }
}
