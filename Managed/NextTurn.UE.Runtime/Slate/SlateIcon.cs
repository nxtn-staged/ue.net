// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Runtime.InteropServices;
using NextTurn.UE;
using NextTurn.UE.Annotations;

namespace Unreal.Slate
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct SlateIcon : IEquatable<SlateIcon>
    {
        private readonly Name styleSetName;
        private readonly Name styleName;
        private readonly Name smallStyleName;
        private readonly NativeBoolean isSet;

        public unsafe SlateIcon(Name styleSetName, Name styleName)
        {
            fixed (SlateIcon* thisPtr = &this)
            {
                NativeMethods.Initialize(thisPtr, styleSetName, styleName);
            }
        }

        public SlateIcon(Name styleSetName, Name styleName, Name smallStyleName)
        {
            this.styleSetName = styleSetName;
            this.styleName = styleName;
            this.smallStyleName = smallStyleName;
            this.isSet = true;
        }

        public Name StyleSetName => this.styleSetName;

        public Name StyleName => this.styleName;

        public Name SmallStyleName => this.smallStyleName;

        public bool IsSet => this.isSet;

        public bool Equals(SlateIcon other) =>
            this.isSet == other.isSet &&
            this.styleSetName == other.styleSetName &&
            this.styleName == other.styleName &&
            this.smallStyleName == other.smallStyleName;

        private static class NativeMethods
        {
            [Calli]
            public static extern unsafe void Initialize(SlateIcon* icon, Name styleSetName, Name styleName);
        }
    }
}
