// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public class Method : CompoundMember
    {
        internal Method(IntPtr pointer) : base(pointer) { }

        public bool HasReturnValue => NativeMethods.GetReturnValueOffset(this.pointer) != ushort.MaxValue;

        /// <summary>
        /// Gets a value indicating whether this <see cref="Method"/> is protected.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this <see cref="Method"/> is family; otherwise, <see langword="false"/>.
        /// </value>
        public bool IsFamily => this.HasAnyFlags(MethodFlags.Family);

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

        [CLSCompliant(false)]
        public MethodFlags MethodFlags => NativeMethods.GetMethodFlags(this.pointer);

        public int ParameterCount => NativeMethods.GetParameterCount(this.pointer);

        internal int ParametersSize => NativeMethods.GetParametersSize(this.pointer);

        public IEnumerable<Property> ParameterProperties => this.EnumerateProperties<Property>();

        public Property? ReturnProperty =>
            !this.HasReturnValue ? null :
            this.ParameterProperties.First(parameter => parameter.IsReturnParameter);

        [CLSCompliant(false)]
        public bool HasAllFlags(MethodFlags flags) => (this.MethodFlags & flags) == flags;

        [CLSCompliant(false)]
        public bool HasAnyFlags(MethodFlags flags) => (this.MethodFlags & flags) != 0;

        internal unsafe void Invoke(Object @object, void* parameters) =>
            NativeMethods.Invoke(this.pointer, @object.pointer, parameters);

        /*
        internal unsafe void InvokeMethod(Method method, params object[] parameters)
        {
            if (parameters.Length != method.ParameterCount)
            {
                throw new NotImplementedException();
            }

            foreach (var property in method.ParameterProperties)
            {
                if (IsReturnOrOutParameter(property))
                {
                }
            }

            byte* parameterBuffer = stackalloc byte[method.ParameterSize];

            NativeMethods.InvokeMethod(this.pointer, method.pointer, new IntPtr(parameterBuffer));

            foreach (var property in method.ParameterProperties)
            {
                if (IsReturnOrOutParameter(property))
                {
                }
            }

            static bool IsReturnOrOutParameter(Property property) =>
                property.IsReturnParameter ||
                (property.HasAnyFlags(PropertyFlags.OutParameter) &&
                !property.HasAnyFlags(PropertyFlags.ByRefParameter));
        }
        */

        internal static new class NativeMethods
        {
            [ReadOffset]
            public static extern MethodFlags GetMethodFlags(IntPtr method);

            [ReadOffset]
            public static extern byte GetParameterCount(IntPtr method);

            [ReadOffset]
            public static extern ushort GetParametersSize(IntPtr method);

            [ReadOffset]
            public static extern ushort GetReturnValueOffset(IntPtr method);

            [Calli]
            public static extern unsafe void Invoke(IntPtr method, IntPtr @object, void* parameters);
        }
    }
}
