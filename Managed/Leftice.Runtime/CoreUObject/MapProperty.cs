// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;
using Leftice.Processors;

namespace Unreal
{
    public sealed class MapProperty : Property, IProperty<ScriptMap>
    {
        internal MapProperty(IntPtr pointer) : base(pointer) { }

        public Property KeyProperty => Create<Property>(NativeMethods.GetKeyProperty(this.pointer));

        public Property ValueProperty => Create<Property>(NativeMethods.GetValueProperty(this.pointer));

        public ScriptMap GetValue(Object @object, int index = 0) => this.GetValue<ScriptMap>(@object, index);

        public void SetValue(Object @object, ScriptMap value, int index = 0) => this.SetValue<ScriptMap>(@object, value, index);

        internal static new class NativeMethods
        {
            [ReadOffset]
            public static extern IntPtr GetKeyProperty(IntPtr property);

            [ReadOffset]
            public static extern IntPtr GetValueProperty(IntPtr property);

            [ReadOffset]
            public static extern ScriptMap.Layout GetLayout(IntPtr property);
        }
    }
}
