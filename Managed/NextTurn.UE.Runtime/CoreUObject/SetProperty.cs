// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public sealed class SetProperty : Property, IProperty<ScriptSet>
    {
        internal SetProperty(IntPtr pointer) : base(pointer) { }

        public Property ItemProperty => Create<Property>(NativeMethods.GetItemProperty(this.pointer));

        public ScriptSet GetValue(Object @object, int index = 0) => this.GetValue<ScriptSet>(@object, index);

        public void SetValue(Object @object, ScriptSet value, int index = 0) => this.SetValue<ScriptSet>(@object, value, index);

        internal static new class NativeMethods
        {
            [ReadOffset]
            public static extern IntPtr GetItemProperty(IntPtr property);

            [ReadOffset]
            public static extern ScriptSet.Layout GetLayout(IntPtr property);
        }
    }
}
