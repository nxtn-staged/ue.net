// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    public class MulticastDelegateProperty : Property, IProperty<MulticastScriptDelegate>
    {
        internal MulticastDelegateProperty(IntPtr pointer) : base(pointer) { }

        public Method SignatureMethod => throw new NotImplementedException();

        public MulticastScriptDelegate GetValue(Object @object, int index = 0) => this.GetValue<MulticastScriptDelegate>(@object, index);

        public void SetValue(Object @object, MulticastScriptDelegate value, int index = 0) => this.SetValue<MulticastScriptDelegate>(@object, value, index);
    }
}
