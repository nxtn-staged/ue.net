// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public sealed class ArrayProperty : Property, IProperty<ScriptArray>
    {
        internal ArrayProperty(IntPtr pointer) : base(pointer) { }

        public Property ItemProperty => Create<Property>(NativeMethods.GetItemProperty(this.pointer));

        public ScriptArray GetValue(Object @object, int index = 0) => this.GetValue<ScriptArray>(@object, index);

        public void SetValue(Object @object, ScriptArray value, int index = 0) => this.SetValue<ScriptArray>(@object, value, index);

        internal static new class NativeMethods
        {
            [ReadOffset]
            public static extern IntPtr GetItemProperty(IntPtr property);
        }
    }
}
