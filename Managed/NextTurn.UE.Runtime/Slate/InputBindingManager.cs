// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using NextTurn.UE.Annotations;

namespace Unreal.Slate
{
    public static class InputBindingManager
    {
        public static unsafe BindingContext GetContext(Name contextName)
        {
            BindingContext result = new BindingContext();
            fixed (SharedReference* contextPtr = &result.reference)
            {
                NativeMethods.GetContext(contextName, contextPtr);
            }

            return result;
        }

        /// <summary>
        /// Removes the <see cref="BindingContext"/> with the specified <paramref name="contextName"/>.
        /// </summary>
        /// <param name="contextName">
        /// The <see cref="Name"/> of the <see cref="BindingContext"/> to remove.
        /// </param>
        public static void RemoveContext(Name contextName) => NativeMethods.RemoveContext(contextName);

        private static class NativeMethods
        {
            [Calli]
            public static extern unsafe void GetContext(Name contextName, SharedReference* context);

            [Calli]
            public static extern void RemoveContext(Name contextName);
        }
    }
}
