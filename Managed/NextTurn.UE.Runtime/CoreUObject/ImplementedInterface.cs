// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using NextTurn.UE;

namespace Unreal
{
    internal struct ImplementedInterface
    {
        internal IntPtr @class;
        internal int pointerOffset;
        internal NativeBoolean implementedByK2;

        internal Class InterfaceClass => Object.Create<Class>(this.@class);
    }
}
