// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    internal readonly struct UniqueObjectGuid : IEquatable<UniqueObjectGuid>
    {
        private readonly Guid guid;

        public bool Equals(UniqueObjectGuid other) => this.guid == other.guid;

        public override int GetHashCode() => throw new NotImplementedException();
    }
}
