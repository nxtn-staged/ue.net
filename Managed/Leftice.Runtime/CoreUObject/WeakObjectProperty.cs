// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    public sealed class WeakObjectProperty : ObjectPropertyBase, IProperty<WeakObjectReference>, IProperty<Object?>
    {
        internal WeakObjectProperty(IntPtr pointer) : base(pointer) { }

        public WeakObjectReference GetValue(Object @object, int index = 0) => this.GetValue<WeakObjectReference>(@object, index);

        unsafe Object? IProperty<Object?>.GetValue(Object @object, int index) => this.GetValuePtr<WeakObjectReference>(@object, index)->Target;

        public void SetValue(Object @object, WeakObjectReference value, int index = 0) => this.SetValue<WeakObjectReference>(@object, value, index);

        void IProperty<Object?>.SetValue(Object @object, Object? value, int index)
        {
            throw new NotImplementedException();
        }
    }
}
