// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;
using System.Collections.Generic;
using Leftice.Processors;

namespace Unreal
{
    public class Method : CompoundMember
    {
        internal Method(IntPtr pointer) : base(pointer) { }

        [CLSCompliant(false)]
        public MethodFlags MethodFlags => NativeMethods.GetMethodFlags(this.pointer);

        /// <summary>
        /// Gets a value indicating whether this <see cref="Method"/> is final.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this <see cref="Method"/> is final; otherwise, <see langword="false"/>.
        /// </value>
        public bool IsFinal => this.HasAnyFlags(MethodFlags.Final);

        /// <summary>
        /// Gets a value indicating whether this <see cref="Method"/> is private.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this <see cref="Method"/> is private; otherwise, <see langword="false"/>.
        /// </value>
        public bool IsPrivate => this.HasAnyFlags(MethodFlags.Private);

        /// <summary>
        /// Gets a value indicating whether this <see cref="Method"/> is protected.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this <see cref="Method"/> is protected; otherwise, <see langword="false"/>.
        /// </value>
        public bool IsProtected => this.HasAnyFlags(MethodFlags.Protected);

        /// <summary>
        /// Gets a value indicating whether this <see cref="Method"/> is public.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this <see cref="Method"/> is public; otherwise, <see langword="false"/>.
        /// </value>
        public bool IsPublic => this.HasAnyFlags(MethodFlags.Public);

        /// <summary>
        /// Gets a value indicating whether this <see cref="Method"/> is static.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this <see cref="Method"/> is static; otherwise, <see langword="false"/>.
        /// </value>
        public bool IsStatic => this.HasAnyFlags(MethodFlags.Static);

        public int ParameterCount =>
            throw new NotImplementedException();

        public IEnumerable<Property> ParameterProperties =>
            throw new NotImplementedException();

        public Property ReturnProperty =>
            throw new NotImplementedException();

        [CLSCompliant(false)]
        public bool HasAllFlags(MethodFlags flags) => (this.MethodFlags & flags) == flags;

        [CLSCompliant(false)]
        public bool HasAnyFlags(MethodFlags flags) => (this.MethodFlags & flags) != 0;

        private static new class NativeMethods
        {
            [ReadOffset]
            public static extern MethodFlags GetMethodFlags(IntPtr function);
        }
    }
}
