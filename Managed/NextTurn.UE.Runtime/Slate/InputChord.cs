// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public struct InputChord : IEquatable<InputChord>
    {
        private readonly Key key;
        private readonly uint modifier;

        public InputChord(Key key)
        {
            this.key = key;
            this.modifier = 0;
        }

        public InputChord(Key key, bool needsShift, bool needsControl, bool needsAlt, bool needsCommand)
        {
            this.key = key;
            this.modifier = 0;

            this.modifier |= needsShift ? NativeMethods.NeedsShift_Mask : 0;
            this.modifier |= needsControl ? NativeMethods.NeedsControl_Mask : 0;
            this.modifier |= needsCommand ? NativeMethods.NeedsCommand_Mask : 0;
            this.modifier |= needsAlt ? NativeMethods.NeedsAlt_Mask : 0;
        }

        public bool IsValid => this.key.IsValid && !this.key.IsModifierKey;

        public Key Key => this.key;

        public bool NeedsAlt => (this.modifier & NativeMethods.NeedsAlt_Mask) != 0;

        public bool NeedsAnyModifierKeys => this.modifier != 0;

        public bool NeedsCommand => (this.modifier & NativeMethods.NeedsCommand_Mask) != 0;

        public bool NeedsControl => (this.modifier & NativeMethods.NeedsControl_Mask) != 0;

        public bool NeedsShift => (this.modifier & NativeMethods.NeedsShift_Mask) != 0;

        public override bool Equals(object? value) => value is InputChord other && this.Equals(other);

        public bool Equals(InputChord other) => this.key.Equals(other.key) && this.modifier == other.modifier;

        public override unsafe int GetHashCode()
        {
            fixed (InputChord* thisPtr = &this)
            {
                return NativeMethods.GetHashCode(thisPtr);
            }
        }

        public static bool operator ==(InputChord left, InputChord right) => left.Equals(right);

        public static bool operator !=(InputChord left, InputChord right) => !(left == right);

        private static class NativeMethods
        {
            public static readonly uint NeedsAlt_Mask;
            public static readonly uint NeedsCommand_Mask;
            public static readonly uint NeedsControl_Mask;
            public static readonly uint NeedsShift_Mask;

            [Calli]
            public static extern unsafe int GetHashCode(InputChord* chord);
        }
    }
}
