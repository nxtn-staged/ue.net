// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    public class NumericProperty<T> : NumericProperty, IProperty<T>
        where T : unmanaged
    {
        internal NumericProperty(IntPtr pointer) : base(pointer) { }

        public T GetValue(Object @object, int index = 0) => this.GetValue<T>(@object, index);

        /// <exception cref="ArgumentNullException"><paramref name="object"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public ref T GetValueRef(Object @object, int index = 0) => ref this.GetValueRef<T>(@object, index);

        public void SetValue(Object @object, T value, int index = 0) => this.SetValue<T>(@object, value, index);
    }
}
