// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    public sealed class LazyObjectProperty : ObjectPropertyBase, IProperty<LazyObjectReference>, IProperty<Object?>
    {
        internal LazyObjectProperty(IntPtr pointer) : base(pointer) { }

        public LazyObjectReference GetValue(Object @object, int index = 0) => this.GetValue<LazyObjectReference>(@object, index);

        unsafe Object? IProperty<Object?>.GetValue(Object @object, int index) => this.GetValuePtr<LazyObjectReference>(@object, index)->Target;

        public void SetValue(Object @object, LazyObjectReference value, int index = 0) => this.SetValue<LazyObjectReference>(@object, value, index);

        void IProperty<Object?>.SetValue(Object @object, Object? value, int index)
        {
            throw new NotImplementedException();
        }
    }
}
