// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;

namespace Unreal
{
    internal sealed class CppStructOperations
    {
        private IntPtr pointer;

        internal CppStructOperations(IntPtr pointer) => this.pointer = pointer;

        public bool HasDestructor => throw new NotImplementedException();

        public bool HasZeroConstructor => throw new NotImplementedException();

        public int Size => throw new NotImplementedException();
    }
}
