// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Unreal
{
    public class Set<T> : IDisposable, ISet<T>, IReadOnlyCollection<T>
    {
        private unsafe ScriptSet* set;
        private bool ownsMemory;

        public unsafe Set()
        {
            if (default(T)! == null && !typeof(T).IsSubclassOf(typeof(Object)))
            {
                throw new NotSupportedException();
            }

            this.set = Memory.Alloc<ScriptSet>();
            this.ownsMemory = true;
        }

        internal unsafe Set(ScriptSet* nativeSet)
        {
            this.set = nativeSet;
            this.ownsMemory = false;
        }

        ~Set() => this.Dispose(false);

        /// <summary>
        /// Gets the number of elements contained in the <see cref="Set{T}"/>.
        /// </summary>
        /// <value>
        /// The number of elements contained in the <see cref="Set{T}"/>.
        /// </value>
        /// <exception cref="ObjectDisposedException">The <see cref="Set{T}"/> has been disposed.</exception>
        public unsafe int Count
        {
            get
            {
                if (this.set is null)
                {
                    this.ThrowObjectDisposedException();
                }

                return this.set->Count;
            }
        }

        bool ICollection<T>.IsReadOnly => false;

        public bool Add(T item)
        {
            throw new NotImplementedException();
        }

        void ICollection<T>.Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
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
                Memory.Free(this.set);
                this.set = null;
                this.ownsMemory = false;
            }
        }

        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        public void ExceptWith(IEnumerable<T> other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (this.Count == 0)
            {
                return;
            }

            if (other == this)
            {
                this.Clear();
                return;
            }

            foreach (T item in other)
            {
                _ = this.Remove(item);
            }
        }

        public Enumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => this.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        public void IntersectWith(IEnumerable<T> other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (this.Count == 0)
            {
                return;
            }

            if (other == this)
            {
                return;
            }
        }

        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (other == this)
            {
                return false;
            }

            return default;
        }

        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (this.Count == 0)
            {
                return false;
            }

            if (other == this)
            {
                return false;
            }

            return default;
        }

        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        public bool IsSubsetOf(IEnumerable<T> other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (this.Count == 0)
            {
                return true;
            }

            if (other == this)
            {
                return true;
            }

            return default;
        }

        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        public bool IsSupersetOf(IEnumerable<T> other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (other == this)
            {
                return true;
            }

            return default;
        }

        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        public bool Overlaps(IEnumerable<T> other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (this.Count == 0)
            {
                return false;
            }

            if (other == this)
            {
                return true;
            }

            foreach (T item in other)
            {
                if (this.Contains(item))
                {
                    return true;
                }
            }

            return false;
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        public bool SetEquals(IEnumerable<T> other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (other == this)
            {
                return true;
            }

            return default;
        }

        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (this.Count == 0)
            {
                this.UnionWith(other);
                return;
            }

            if (other == this)
            {
                this.Clear();
                return;
            }
        }

        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        public void UnionWith(IEnumerable<T> other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            foreach (T item in other)
            {
                _ = this.Add(item);
            }
        }

        [DoesNotReturn]
        private void ThrowObjectDisposedException() => throw new ObjectDisposedException(this.GetType().Name);

        public struct Enumerator : IEnumerator<T>
        {
            public T Current => throw new NotImplementedException();

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
