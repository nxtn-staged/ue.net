// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Unreal
{
    [DebuggerDisplay("Count = {Count}")]
    public class Map<TKey, TValue> : IDisposable, IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
        where TKey : notnull
    {
        private readonly ScriptMapHelper helper;
        private unsafe ScriptMap* map;
        private bool ownsMemory;

        /// <exception cref="NotSupportedException">
        /// The type of this instance (<typeparamref name="TKey"/> or <typeparamref name="TValue"/>) is not supported.
        /// </exception>
        public unsafe Map()
        {
            if (!NextTurn.UE.Marshaler<TKey>.IsSupported ||
                !NextTurn.UE.Marshaler<TValue>.IsSupported)
            {
                Throw.NotSupportedException();
            }

            this.map = Memory.Alloc<ScriptMap>();
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
            get
            {
                IntPtr valuePtr = this.helper.FindValuePtr(key);
                return valuePtr == IntPtr.Zero ? throw new KeyNotFoundException() : NextTurn.UE.Marshaler<TValue>.ToManaged(valuePtr, 0);
            }

            set => throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the number of elements contained in this <see cref="Map{TKey, TValue}"/>.
        /// </summary>
        /// <value>
        /// The number of elements contained in this <see cref="Map{TKey, TValue}"/>.
        /// </value>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="Map{TKey, TValue}"/> has been disposed.
        /// </exception>
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

        public KeyCollection Keys => new KeyCollection(this);

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => this.Keys;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => this.Keys;

        public ValueCollection Values => new ValueCollection(this);

        ICollection<TValue> IDictionary<TKey, TValue>.Values => this.Values;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => this.Values;

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        public void Add(TKey key, TValue value)
        {
            this.helper.Add(key, value);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this.Add(item.Key, item.Value);
        }

        public unsafe void Clear()
        {
            if (this.map is null)
            {
                this.ThrowObjectDisposedException();
            }

            this.helper.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(TKey key) => this.helper.ContainsKey(key);

        public bool ContainsValue(TValue value)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array is null)
            {
                Throw.ArrayArgumentNullException();
            }
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

        public Enumerator GetEnumerator() => new Enumerator(this);

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => this.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        [return: MaybeNull]
        public TValue GetValueOrDefault(TKey key) => this.GetValueOrDefault(key, default);

        [return: MaybeNull]
        public TValue GetValueOrDefault(TKey key, [AllowNull] TValue defaultValue) =>
            this.TryGetValue(key, out TValue value) ? value : defaultValue;

        public bool Remove(TKey key) => this.helper.Remove(key);

        public bool Remove(TKey key, [MaybeNullWhen(false)] out TValue value) => this.helper.Remove(key, out value);

        public bool Remove(KeyValuePair<TKey, TValue> item) => this.helper.Remove(item);

        [DoesNotReturn]
        private void ThrowObjectDisposedException() => throw new ObjectDisposedException(this.GetType().Name);

        public bool TryAdd(TKey key, TValue value)
        {
            this.Add(key, value);
            return true;
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            IntPtr valuePtr = this.helper.FindValuePtr(key);
            if (valuePtr != IntPtr.Zero)
            {
                value = NextTurn.UE.Marshaler<TValue>.ToManaged(valuePtr, 0);
                return true;
            }

            value = default!;
            return false;
        }

        private enum AdditionBehavior
        {
            None,
            OverwriteExisting,
            ThrowOnExisting,
        }

        public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private readonly Map<TKey, TValue> map;

            internal Enumerator(Map<TKey, TValue> map)
            {
                this.map = map;
            }

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

        public struct KeyCollection : ICollection<TKey>
        {
            private readonly Map<TKey, TValue> map;

            internal KeyCollection(Map<TKey, TValue> map) => this.map = map;

            public int Count => this.map.Count;

            bool ICollection<TKey>.IsReadOnly => true;

            void ICollection<TKey>.Add(TKey item) => Throw.NotSupportedException();

            void ICollection<TKey>.Clear() => Throw.NotSupportedException();

            public bool Contains(TKey item) => this.map.ContainsKey(item);

            public void CopyTo(TKey[] array, int arrayIndex)
            {
                if (array is null)
                {
                    Throw.ArrayArgumentNullException();
                }
            }

            public Enumerator GetEnumerator() => new Enumerator(this.map);

            IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator() => this.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            bool ICollection<TKey>.Remove(TKey item)
            {
                Throw.NotSupportedException();
                return default;
            }

            public struct Enumerator : IEnumerator<TKey>
            {
                private readonly Map<TKey, TValue> map;

                internal Enumerator(Map<TKey, TValue> map)
                {
                    this.map = map;
                }

                public TKey Current => throw new NotImplementedException();

                object? IEnumerator.Current => throw new NotImplementedException();

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

        public struct ValueCollection : ICollection<TValue>
        {
            private readonly Map<TKey, TValue> map;

            internal ValueCollection(Map<TKey, TValue> map) => this.map = map;

            public int Count => this.map.Count;

            bool ICollection<TValue>.IsReadOnly => true;

            void ICollection<TValue>.Add(TValue item) => Throw.NotSupportedException();

            void ICollection<TValue>.Clear() => Throw.NotSupportedException();

            public bool Contains(TValue item) => this.map.ContainsValue(item);

            public void CopyTo(TValue[] array, int arrayIndex)
            {
                if (array is null)
                {
                    Throw.ArrayArgumentNullException();
                }
            }

            public Enumerator GetEnumerator() => new Enumerator(this.map);

            IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator() => this.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            bool ICollection<TValue>.Remove(TValue item)
            {
                Throw.NotSupportedException();
                return default;
            }

            public struct Enumerator : IEnumerator<TValue>
            {
                private readonly Map<TKey, TValue> map;

                internal Enumerator(Map<TKey, TValue> map)
                {
                    this.map = map;
                }

                public TValue Current => throw new NotImplementedException();

                object? IEnumerator.Current => throw new NotImplementedException();

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
}
