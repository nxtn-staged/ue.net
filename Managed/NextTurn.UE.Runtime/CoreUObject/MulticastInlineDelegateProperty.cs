// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    public sealed class MulticastInlineDelegateProperty : MulticastDelegateProperty
    {
        internal MulticastInlineDelegateProperty(IntPtr pointer) : base(pointer) { }

        public override MulticastScriptDelegate GetValue(Object @object, int index = 0) => this.GetValue<MulticastScriptDelegate>(@object, index);

        public override void SetValue(Object @object, MulticastScriptDelegate value, int index = 0) => this.SetValue<MulticastScriptDelegate>(@object, value, index);
    }
}
