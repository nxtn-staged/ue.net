// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Unreal
{
    public class Map<TKey, TValue> : IDisposable, IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue> where TKey : notnull
    {
        private unsafe ScriptMap* map;
        private bool ownsMemory;

        public unsafe Map()
        {
            if (default(TKey)! == null && !typeof(TKey).IsSubclassOf(typeof(Object)) ||
                default(TValue)! == null && !typeof(TValue).IsSubclassOf(typeof(Object)))
            {
                throw new NotSupportedException();
            }

            this.map = null;
            this.ownsMemory = true;
        }

        internal unsafe Map(ScriptMap* nativeMap)
        {
            this.map = nativeMap;
            this.ownsMemory = false;
        }

        ~Map() => this.Dispose(false);

        public TValue this[TKey key]
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="Map{TKey, TValue}"/>.
        /// </summary>
        /// <value>
        /// The number of elements contained in the <see cref="Map{TKey, TValue}"/>.
        /// </value>
        /// <exception cref="ObjectDisposedException">The <see cref="Map{TKey, TValue}"/> has been disposed.</exception>
        public unsafe int Count
        {
            get
            {
                if (this.map is null)
                {
                    this.ThrowObjectDisposedException();
                }

                return this.map->Count;
            }
        }

        public ICollection<TKey> Keys => throw new NotImplementedException();

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => throw new NotImplementedException();

        public ICollection<TValue> Values => throw new NotImplementedException();

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => throw new NotImplementedException();

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        public void Add(TKey key, TValue value) => this.Add(new KeyValuePair<TKey, TValue>(key, value));

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(TKey key)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual unsafe void Dispose(bool disposing)
        {
            if (this.ownsMemory)
            {
                Memory.Free(this.map);
                this.map = null;
                this.ownsMemory = false;
            }
        }

        public Enumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => this.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public bool Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            throw new NotImplementedException();
        }

        [DoesNotReturn]
        private void ThrowObjectDisposedException() => throw new ObjectDisposedException(this.GetType().Name);

        public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            public KeyValuePair<TKey, TValue> Current => throw new NotImplementedException();

            object? IEnumerator.Current => this.Current;

            public void Dispose() { }

            public bool MoveNext()
            {
                throw new NotImplementedException();
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }
        }
    }
}
