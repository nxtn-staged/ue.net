// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public struct Key : IEquatable<Key>
    {
        private readonly Name name;
        private readonly SharedReference unused;

        public unsafe bool IsFloatAxis
        {
            get
            {
                fixed (Key* thisPtr = &this)
                {
                    return NativeMethods.IsFloatAxis(thisPtr);
                }
            }
        }

        public unsafe bool IsModifierKey
        {
            get
            {
                fixed (Key* thisPtr = &this)
                {
                    return NativeMethods.IsModifierKey(thisPtr);
                }
            }
        }

        public unsafe bool IsMouseButton
        {
            get
            {
                fixed (Key* thisPtr = &this)
                {
                    return NativeMethods.IsMouseButton(thisPtr);
                }
            }
        }

        public unsafe bool IsValid
        {
            get
            {
                fixed (Key* thisPtr = &this)
                {
                    return NativeMethods.IsValid(thisPtr);
                }
            }
        }

        public unsafe bool IsVectorAxis
        {
            get
            {
                fixed (Key* thisPtr = &this)
                {
                    return NativeMethods.IsVectorAxis(thisPtr);
                }
            }
        }

        public Name Name => this.name;

        public override bool Equals(object? value) => value is Key other && this.Equals(other);

        public bool Equals(Key other) => this.name.Equals(other.name);

        public override int GetHashCode() => this.name.GetHashCode();

        public static bool operator ==(Key left, Key right) => left.name.Equals(right.name);

        public static bool operator !=(Key left, Key right) => !(left == right);

        private static class NativeMethods
        {
            [Calli]
            public static extern unsafe bool IsFloatAxis(Key* key);

            [Calli]
            public static extern unsafe bool IsModifierKey(Key* key);

            [Calli]
            public static extern unsafe bool IsMouseButton(Key* key);

            [Calli]
            public static extern unsafe bool IsValid(Key* key);

            [Calli]
            public static extern unsafe bool IsVectorAxis(Key* key);
        }
    }
}
