// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;

namespace Unreal
{
    public struct ScriptDelegate : IEquatable<ScriptDelegate>
    {
        private WeakObjectReference targetReference;
        private Name functionName;

        public Name FunctionName => this.functionName;

        public Object? Target => this.targetReference.Target;

        internal void Invoke(IntPtr parameters)
        {
            if (!this.targetReference.IsValid || this.functionName.IsNone)
            {
                throw new InvalidOperationException();
            }
        }

        public bool Equals(ScriptDelegate other) => this.targetReference == other.targetReference && this.functionName == other.functionName;
    }
}
