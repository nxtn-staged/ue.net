// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace NextTurn.UE.Programs
{
    internal static class Singleton
    {
        internal static IEnumerable<T> Enumerable<T>(T value) => new Collection<T>(value);

        private sealed class Collection<T> : ICollection<T>
        {
            private readonly T value;

            public Collection(T value) => this.value = value;

            public int Count => 1;

            public bool IsReadOnly => true;

            public void Add(T item) => throw new NotSupportedException();

            public void Clear() => throw new NotSupportedException();

            public bool Contains(T item) => EqualityComparer<T>.Default.Equals(this.value, item);

            public void CopyTo(T[] array, int arrayIndex) => array[arrayIndex] = this.value;

            public IEnumerator<T> GetEnumerator() => new Enumerator(this.value);

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            public bool Remove(T item) => throw new NotSupportedException();

            private sealed class Enumerator : IEnumerator<T>
            {
                private bool moved;

                internal Enumerator(T value) => this.Current = value;

                public T Current { get; }

                object? IEnumerator.Current => this.Current;

                public void Dispose()
                {
                }

                public bool MoveNext() => !this.moved && (this.moved = true);

                public void Reset() => this.moved = false;
            }
        }
    }
}
