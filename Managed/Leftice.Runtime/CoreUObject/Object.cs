// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;
using System.Diagnostics;
using Leftice;
using Leftice.Processors;

namespace Unreal
{
    public class Object
    {
        internal IntPtr pointer;

        internal Object(IntPtr pointer)
        {
            Debug.Assert(pointer != IntPtr.Zero);

            this.pointer = pointer;
        }

        public Class Class => new Class(NativeMethods.GetClass(this.pointer));

        public Name Name => NativeMethods.GetName(this.pointer);

        public bool IsPendingDestruction
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public Package Package => throw new NotImplementedException();

        public static T? Find<T>(Object? declaringObject, string? name) where T : Object => default;

        internal static IntPtr GetPointerOrDefault(Object? @object) =>
            @object?.pointer ?? IntPtr.Zero;

        internal static IntPtr GetPointerOrThrow(Object @object) =>
            @object?.pointer ?? throw new ArgumentNullException(nameof(@object));

        public void AddToRoot() => throw new NotImplementedException();

        internal void InvokeFunction(Function function)
        {
            foreach (var property in function.ParameterProperties)
            {
                if (IsReturnOrOutParameter(property))
                {
                }
            }

            NativeMethods.InvokeFunction(function.pointer, default);

            foreach (var property in function.ParameterProperties)
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

        public void RemoveFromRoot() => throw new NotImplementedException();

        internal static class NativeMethods
        {
            [ReadOffset]
            public static extern IntPtr GetClass(IntPtr @object);

            [ReadOffset]
            public static extern Name GetName(IntPtr @object);

            [Calli]
            public static extern void InvokeFunction(IntPtr function, IntPtr parameters);
        }
    }
}
