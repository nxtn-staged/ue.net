// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    public struct MulticastScriptDelegate
    {
        private ScriptArray invocationList;

        internal unsafe void Invoke(void* parameters)
        {
            int count = this.invocationList.Count;
            if (count == 0)
            {
                return;
            }

            Span<ScriptDelegate> invocationListCopy = count <= 4
                ? stackalloc ScriptDelegate[count] : new ScriptDelegate[count];

            this.invocationList.AsSpan<ScriptDelegate>().CopyTo(invocationListCopy);

            foreach (ScriptDelegate @delegate in invocationListCopy)
            {
                @delegate.InvokeInternal(parameters);
            }
        }
    }
}
