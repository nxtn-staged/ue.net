// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using NextTurn.UE.Annotations;

namespace Unreal.Slate
{
    public sealed class TagMetaData : MetaData
    {
        private protected override unsafe void Initialize(SharedReference* reference) =>
            NativeMethods.Initialize(reference);

        private static class NativeMethods
        {
            [Calli]
            public static extern unsafe void Initialize(SharedReference* metaData);
        }
    }
}
