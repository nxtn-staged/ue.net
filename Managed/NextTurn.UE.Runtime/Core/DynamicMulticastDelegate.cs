// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

namespace Unreal
{
    public abstract class DynamicMulticastDelegate
    {
        private readonly unsafe MulticastScriptDelegate* @delegate;

        public void Combine(in ScriptDelegate @delegate)
        {
        }

        private protected unsafe void InvokeInternal(void* parameters) => this.@delegate->Invoke(parameters);

        public void Remove(in ScriptDelegate @delegate)
        {
        }
    }
}
