// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    public class SoftObjectProperty : ObjectPropertyBase, IProperty<SoftObjectReference>, IProperty<Object?>
    {
        internal SoftObjectProperty(IntPtr pointer) : base(pointer) { }

        public SoftObjectReference GetValue(Object @object, int index = 0) => this.GetValue<SoftObjectReference>(@object, index);

        unsafe Object? IProperty<Object?>.GetValue(Object @object, int index) => this.GetValuePtr<SoftObjectReference>(@object, index)->Target;

        public void SetValue(Object @object, SoftObjectReference value, int index = 0) => this.SetValue<SoftObjectReference>(@object, value, index);

        void IProperty<Object?>.SetValue(Object @object, Object? value, int index)
        {
            throw new NotImplementedException();
        }
    }
}
