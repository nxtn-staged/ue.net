// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public sealed class InterfaceProperty : Property, IProperty<ScriptInterface>
    {
        internal InterfaceProperty(IntPtr pointer) : base(pointer) { }

        public Class InterfaceClass => Object.Create<Class>(NativeMethods.GetInterfaceClass(this.pointer));

        public ScriptInterface GetValue(Object @object, int index = 0) => this.GetValue<ScriptInterface>(@object, index);

        public void SetValue(Object @object, ScriptInterface value, int index = 0) => this.SetValue<ScriptInterface>(@object, value, index);

        private static new class NativeMethods
        {
            [ReadOffset]
            public static extern IntPtr GetInterfaceClass(IntPtr property);
        }
    }
}
