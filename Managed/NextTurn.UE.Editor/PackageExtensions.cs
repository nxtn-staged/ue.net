// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal.Editor
{
    public static class PackageExtensions
    {
        /// <exception cref="ArgumentNullException"><paramref name="package"/> is <see langword="null"/>.</exception>
        public static MetaData GetMetaData(this Package package) =>
            package is null ? throw new ArgumentNullException(nameof(package)) :
            default(MetaData)!;
    }
}
