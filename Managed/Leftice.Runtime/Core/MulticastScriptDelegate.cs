// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;

namespace Unreal
{
    public struct MulticastScriptDelegate
    {
        private ScriptArray invocationList;

        internal void Invoke(IntPtr parameters)
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
                @delegate.Invoke(parameters);
            }
        }
    }
}
