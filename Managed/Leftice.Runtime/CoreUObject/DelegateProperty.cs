// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

using System;
using Leftice.Processors;

namespace Unreal
{
    public sealed class DelegateProperty : Property, IProperty<ScriptDelegate>
    {
        internal DelegateProperty(IntPtr pointer) : base(pointer) { }

        public DelegateMethod SignatureMethod => Create<DelegateMethod>(NativeMethods.GetSignatureMethod(this.pointer));

        public ScriptDelegate GetValue(Object @object, int index = 0) => this.GetValue<ScriptDelegate>(@object, index);

        public void SetValue(Object @object, ScriptDelegate value, int index = 0) => this.SetValue<ScriptDelegate>(@object, value, index);

        private static new class NativeMethods
        {
            [ReadOffset]
            public static extern IntPtr GetSignatureMethod(IntPtr property);
        }
    }
}
