// Copyright (c) NextTurn.
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
