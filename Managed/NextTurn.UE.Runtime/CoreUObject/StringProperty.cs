// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    public sealed class StringProperty : Property, IProperty<string>
    {
        internal StringProperty(IntPtr pointer) : base(pointer) { }

        public string GetValue(Object @object, int index = 0)
        {
            throw new NotImplementedException();
        }

        public void SetValue(Object @object, string value, int index = 0)
        {
            throw new NotImplementedException();
        }
    }
}
