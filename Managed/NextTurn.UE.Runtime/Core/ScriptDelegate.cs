// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    public readonly struct ScriptDelegate : IDynamicDelegate, IEquatable<ScriptDelegate>
    {
        private readonly WeakObjectReference targetReference;
        private readonly Name methodName;

        public ScriptDelegate(Object target, Name methodName)
        {
            this.targetReference = new WeakObjectReference(target);
            this.methodName = methodName;
        }

        public Name MethodName => this.methodName;

        public Object? Target => this.targetReference.Target;

        /// <exception cref="InvalidOperationException">
        /// <see cref="Target"/> is <see langword="null"/>.
        /// -or-
        /// <see cref="MethodName"/> is <see cref="Name.None"/>.
        /// </exception>
        internal unsafe void InvokeInternal(void* parameters)
        {
            if (!this.targetReference.IsValid || this.methodName.IsNone)
            {
                Throw.InvalidOperationException();
            }

            IntPtr target = WeakObjectReference.NativeMethods.GetTarget(this.targetReference);
            IntPtr @class = Object.NativeMethods.GetClass(target);
            IntPtr method = Class.NativeMethods.FindMethod(@class, this.methodName);
            if (method == IntPtr.Zero)
            {
                Throw.InvalidOperationException();
            }

            Method.NativeMethods.Invoke(method, target, parameters);
        }

        public bool Equals(ScriptDelegate other) => this.targetReference == other.targetReference && this.methodName == other.methodName;
    }
}
