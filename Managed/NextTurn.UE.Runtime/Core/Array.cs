// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Unreal
{
    [DebuggerDisplay("Count = {Count}")]
    public class Array<T> : IDisposable, IList<T>, IReadOnlyList<T>
        where T : IEquatable<T>
    {
        private readonly ScriptArrayHelper helper;
        internal unsafe ScriptArray* array;
        private bool ownsMemory;

        /// <exception cref="NotSupportedException">
        /// The type of this instance (<typeparamref name="T"/>) is not supported.
        /// </exception>
        public unsafe Array()
        {
            if (!NextTurn.UE.Marshaler<T>.IsSupported)
            {
                Throw.NotSupportedException();
            }

            this.array = Memory.Alloc<ScriptArray>();
            this.ownsMemory = true;
        }

        internal unsafe Array(ScriptArray* nativeArray)
        {
            this.array = nativeArray;
            this.ownsMemory = false;
        }

        ~Array() => this.Dispose(false);

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the element to get or set.
        /// </param>
        /// <value>
        /// The element at the specified index.
        /// </value>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0.
        /// -or-
        /// <paramref name="index"/> is greater than or equal to <see cref="Count"/>.
        /// </exception>
        /// <exception cref="ObjectDisposedException">This <see cref="Array{T}"/> has been disposed.</exception>
        public unsafe T this[int index]
        {
            get
            {
                if (this.array is null)
                {
                    this.ThrowObjectDisposedException();
                }

                if ((uint)index >= (uint)this.array->Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return NextTurn.UE.Marshaler<T>.ToManaged(new IntPtr(this.array->GetItemPtrUnsafe<T>(index)), default);
            }

            set
            {
                if ((uint)index >= (uint)this.array->Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                NextTurn.UE.Marshaler<T>.ToNative(new IntPtr(this.array->GetItemPtrUnsafe<T>(index)), default, value);
            }
        }

        /// <summary>
        /// Gets the number of elements that this <see cref="Array{T}"/> can contain.
        /// </summary>
        /// <value>
        /// The number of elements that this <see cref="Array{T}"/> can contain.
        /// </value>
        /// <exception cref="ObjectDisposedException">This <see cref="Array{T}"/> has been disposed.</exception>
        public unsafe int Capacity
        {
            get
            {
                if (this.array is null)
                {
                    this.ThrowObjectDisposedException();
                }

                return this.array->Capacity;
            }
        }

        /// <summary>
        /// Gets the number of elements contained in this <see cref="Array{T}"/>.
        /// </summary>
        /// <value>
        /// The number of elements contained in this <see cref="Array{T}"/>.
        /// </value>
        /// <exception cref="ObjectDisposedException">This <see cref="Array{T}"/> has been disposed.</exception>
        public unsafe int Count
        {
            get
            {
                if (this.array is null)
                {
                    this.ThrowObjectDisposedException();
                }

                return this.array->Count;
            }
        }

        bool ICollection<T>.IsReadOnly => false;

        /// <summary>
        /// Adds an item to the end.
        /// </summary>
        /// <param name="item">
        /// The item to be added to the end.
        /// </param>
        /// <seealso cref="AddRange"/>
        /// <seealso cref="Insert"/>
        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        /// <seealso cref="Add"/>
        public void AddRange(IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                this.Add(item);
            }
        }

        public unsafe bool All(Predicate<T> predicate)
        {
            for (int i = 0; i < this.Count; i++)
            {
                T item = this.array->GetItemUnsafe<T>(i);
                if (!predicate(item))
                {
                    return false;
                }
            }

            return true;
        }

        public unsafe bool Any(Predicate<T> predicate)
        {
            for (int i = 0; i < this.Count; i++)
            {
                T item = this.array->GetItemUnsafe<T>(i);
                if (predicate(item))
                {
                    return true;
                }
            }

            return false;
        }

        private unsafe ReadOnlySpan<T> AsSpan() =>
            new ReadOnlySpan<T>(this.array->Items, this.Count);

        private unsafe ReadOnlySpan<T> AsSpan(int start) =>
            new ReadOnlySpan<T>(Unsafe.Add<T>(this.array->Items, start), this.Count - start);

        private unsafe ReadOnlySpan<T> AsSpan(int start, int count) =>
            new ReadOnlySpan<T>(Unsafe.Add<T>(this.array->Items, start), count);

        private unsafe ReadOnlySpan<T> AsSpanFromEnd(int start) =>
            new ReadOnlySpan<T>(this.array->Items, start + 1);

        private unsafe ReadOnlySpan<T> AsSpanFromEnd(int start, int count) =>
            new ReadOnlySpan<T>(Unsafe.Add<T>(this.array->Items, start - count + 1), count);

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item) => this.AsSpan().Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => this.AsSpan().CopyTo(array.AsSpan(arrayIndex));

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual unsafe void Dispose(bool disposing)
        {
            if (this.ownsMemory)
            {
                Memory.Free(this.array);
                this.array = null;
                this.ownsMemory = false;
            }
        }

        /// <exception cref="ArgumentNullException">
        /// <paramref name="match"/> is <see langword="null"/>.
        /// </exception>
        /// <seealso cref="FindIndex"/>
        /// <seealso cref="FindLast"/>
        /// <seealso cref="FindLastIndex"/>
        /// <seealso cref="IndexOf"/>
        /// <seealso cref="LastIndexOf"/>
        public unsafe T Find(Predicate<T> match)
        {
            if (match is null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            for (int i = 0; i < this.Count; i++)
            {
                T item = this.array->GetItemUnsafe<T>(i);
                if (match(item))
                {
                    return item;
                }
            }

            return default;
        }

        /// <exception cref="ArgumentNullException">
        /// <paramref name="match"/> is <see langword="null"/>.
        /// </exception>
        /// <seealso cref="Find"/>
        /// <seealso cref="FindLast"/>
        /// <seealso cref="FindLastIndex"/>
        /// <seealso cref="IndexOf"/>
        /// <seealso cref="LastIndexOf"/>
        public int FindIndex(Predicate<T> match) => this.FindIndex(0, match);

        /// <exception cref="ArgumentNullException">
        /// <paramref name="match"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <seealso cref="Find"/>
        /// <seealso cref="FindLast"/>
        /// <seealso cref="FindLastIndex"/>
        /// <seealso cref="IndexOf"/>
        /// <seealso cref="LastIndexOf"/>
        public int FindIndex(Index start, Predicate<T> match) => this.FindIndex(start.GetOffset(this.Count), match);

        /// <exception cref="ArgumentNullException">
        /// <paramref name="match"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <seealso cref="Find"/>
        /// <seealso cref="FindLast"/>
        /// <seealso cref="FindLastIndex"/>
        /// <seealso cref="IndexOf"/>
        /// <seealso cref="LastIndexOf"/>
        public int FindIndex(int start, Predicate<T> match) => this.FindIndex(start, this.Count - start, match);

        /// <exception cref="ArgumentNullException">
        /// <paramref name="match"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <seealso cref="Find"/>
        /// <seealso cref="FindLast"/>
        /// <seealso cref="FindLastIndex"/>
        /// <seealso cref="IndexOf"/>
        /// <seealso cref="LastIndexOf"/>
        public unsafe int FindIndex(Range range, Predicate<T> match)
        {
            (int start, int count) = range.GetOffsetAndLength(this.Count);

            if (match is null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            for (int i = start; i < start + count; i++)
            {
                if (match(this.array->GetItemUnsafe<T>(i)))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <exception cref="ArgumentNullException">
        /// <paramref name="match"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <seealso cref="Find"/>
        /// <seealso cref="FindLast"/>
        /// <seealso cref="FindLastIndex"/>
        /// <seealso cref="IndexOf"/>
        /// <seealso cref="LastIndexOf"/>
        public unsafe int FindIndex(int start, int count, Predicate<T> match)
        {
            if (Environment.Is64BitProcess)
            {
                if ((ulong)(uint)start + (uint)count > (uint)this.Count)
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                if ((uint)start > (uint)this.Count || (uint)count > (uint)(this.Count - start))
                {
                    throw new ArgumentOutOfRangeException();
                }
            }

            if (match is null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            for (int i = start; i < start + count; i++)
            {
                if (match(this.array->GetItemUnsafe<T>(i)))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <exception cref="ArgumentNullException">
        /// <paramref name="match"/> is <see langword="null"/>.
        /// </exception>
        /// <seealso cref="Find"/>
        /// <seealso cref="FindIndex"/>
        /// <seealso cref="FindLastIndex"/>
        /// <seealso cref="IndexOf"/>
        /// <seealso cref="LastIndexOf"/>
        public unsafe T FindLast(Predicate<T> match)
        {
            if (match is null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            for (int i = this.Count - 1; i >= 0; i--)
            {
                T item = this.array->GetItemUnsafe<T>(i);
                if (match(item))
                {
                    return item;
                }
            }

            return default;
        }

        /// <exception cref="ArgumentNullException">
        /// <paramref name="match"/> is <see langword="null"/>.
        /// </exception>
        /// <seealso cref="Find"/>
        /// <seealso cref="FindIndex"/>
        /// <seealso cref="FindLast"/>
        /// <seealso cref="IndexOf"/>
        /// <seealso cref="LastIndexOf"/>
        public int FindLastIndex(Predicate<T> match) => this.FindLastIndex(this.Count - 1, match);

        /// <exception cref="ArgumentNullException">
        /// <paramref name="match"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <seealso cref="Find"/>
        /// <seealso cref="FindIndex"/>
        /// <seealso cref="FindLast"/>
        /// <seealso cref="IndexOf"/>
        /// <seealso cref="LastIndexOf"/>
        public int FindLastIndex(Index start, Predicate<T> match) => this.FindLastIndex(start.GetOffset(this.Count), match);

        /// <exception cref="ArgumentNullException">
        /// <paramref name="match"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <seealso cref="Find"/>
        /// <seealso cref="FindIndex"/>
        /// <seealso cref="FindLast"/>
        /// <seealso cref="IndexOf"/>
        /// <seealso cref="LastIndexOf"/>
        public int FindLastIndex(int start, Predicate<T> match) => this.FindLastIndex(start, start + 1, match);

        /// <exception cref="ArgumentNullException">
        /// <paramref name="match"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <seealso cref="Find"/>
        /// <seealso cref="FindIndex"/>
        /// <seealso cref="FindLast"/>
        /// <seealso cref="IndexOf"/>
        /// <seealso cref="LastIndexOf"/>
        public unsafe int FindLastIndex(Range range, Predicate<T> match)
        {
            (int start, int count) = range.GetOffsetAndLength(this.Count);

            if (match is null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            for (int i = start; i > start - count; i--)
            {
                if (match(this.array->GetItemUnsafe<T>(i)))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <exception cref="ArgumentNullException">
        /// <paramref name="match"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <seealso cref="Find"/>
        /// <seealso cref="FindIndex"/>
        /// <seealso cref="FindLast"/>
        /// <seealso cref="IndexOf"/>
        /// <seealso cref="LastIndexOf"/>
        public unsafe int FindLastIndex(int start, int count, Predicate<T> match)
        {
            if (this.Count == 0)
            {
                if (start != -1)
                {
                    throw new ArgumentOutOfRangeException(nameof(start));
                }
            }
            else
            {
                if ((uint)start >= (uint)this.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(start));
                }
            }

            if ((uint)count > (uint)(start + 1))
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (match is null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            for (int i = start; i > start - count; i--)
            {
                if (match(this.array->GetItemUnsafe<T>(i)))
                {
                    return i;
                }
            }

            return -1;
        }

        public T First() => this[0];

        public Enumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => this.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        /// <summary>
        /// Searches for the specified item and returns the zero-based index of its first occurence.
        /// </summary>
        /// <param name="item">
        /// The item to search for.
        /// </param>
        /// <returns>
        /// The zero-based index of the first occurence of item, if found; otherwise, -1.
        /// </returns>
        /// <seealso cref="LastIndexOf"/>
        public int IndexOf(T item) => this.AsSpan().IndexOf(item);

        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <seealso cref="LastIndexOf"/>
        public int IndexOf(T item, Index start) => this.IndexOf(item, start.GetOffset(this.Count));

        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <seealso cref="LastIndexOf"/>
        public int IndexOf(T item, int start) =>
            (uint)start > (uint)this.Count ? throw new ArgumentOutOfRangeException(nameof(start)) :
            this.AsSpan(start).IndexOf(item);

        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <seealso cref="LastIndexOf"/>
        public int IndexOf(T item, Range range)
        {
            (int start, int count) = range.GetOffsetAndLength(this.Count);
            return this.AsSpan(start, count).IndexOf(item);
        }

        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <seealso cref="LastIndexOf"/>
        public int IndexOf(T item, int start, int count)
        {
            if (Environment.Is64BitProcess)
            {
                if ((ulong)(uint)start + (uint)count > (uint)this.Count)
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                if ((uint)start > (uint)this.Count || (uint)count > (uint)(this.Count - start))
                {
                    throw new ArgumentOutOfRangeException();
                }
            }

            return this.AsSpan(start, count).IndexOf(item);
        }

        public T Last() => this[^1];

        /// <summary>
        /// Inserts an item at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index at which item should be inserted.
        /// </param>
        /// <param name="item">
        /// The item to be inserted.
        /// </param>
        /// <seealso cref="Add"/>
        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Searches for the specified item and returns the zero-based index of its last occurence.
        /// </summary>
        /// <param name="item">
        /// The item to search for.
        /// </param>
        /// <returns>
        /// The zero-based index of the last occurence of item, if found; otherwise, -1.
        /// </returns>
        /// <seealso cref="IndexOf"/>
        public int LastIndexOf(T item) => this.AsSpan().LastIndexOf(item);

        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <seealso cref="IndexOf"/>
        public int LastIndexOf(T item, Index start) => this.LastIndexOf(item, start.GetOffset(this.Count));

        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <seealso cref="IndexOf"/>
        public int LastIndexOf(T item, int start)
        {
            if (this.Count == 0)
            {
                if (start != -1)
                {
                    throw new ArgumentOutOfRangeException(nameof(start));
                }
            }
            else
            {
                if ((uint)start >= (uint)this.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(start));
                }
            }

            return this.AsSpanFromEnd(start).LastIndexOf(item);
        }

        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <seealso cref="IndexOf"/>
        public int LastIndexOf(T item, Range range)
        {
            (int end, int count) = range.GetOffsetAndLength(this.Count);
            return this.AsSpan(end, count).LastIndexOf(item);
        }

        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <seealso cref="IndexOf"/>
        public int LastIndexOf(T item, int start, int count)
        {
            if (this.Count == 0)
            {
                if (start != -1)
                {
                    throw new ArgumentOutOfRangeException(nameof(start));
                }
            }
            else
            {
                if ((uint)start >= (uint)this.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(start));
                }
            }

            if ((uint)count > (uint)(start + 1))
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            return this.AsSpanFromEnd(start, count).LastIndexOf(item);
        }

        /// <summary>
        /// Removes the first occurence of the specified item.
        /// </summary>
        /// <param name="item">
        /// The item to remove.
        /// </param>
        /// <returns>
        /// <see langword="true"/> is item is successfully removed; otherwise, <see langword="false"/>.
        /// </returns>
        /// <seealso cref="RemoveAt"/>
        /// <seealso cref="RemoveRange"/>
        public bool Remove(T item)
        {
            int index = this.IndexOf(item);
            if (index >= 0)
            {
                this.RemoveAt(index);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes the element at the specified index.
        /// </summary>
        /// <param name="index">
        /// The index of the element to remove.
        /// </param>
        /// <seealso cref="Remove"/>
        /// <seealso cref="RemoveRange"/>
        public void RemoveAt(Index index) => this.RemoveAt(index.GetOffset(this.Count));

        /// <summary>
        /// Removes the element at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the element to remove.
        /// </param>
        /// <seealso cref="Remove"/>
        /// <seealso cref="RemoveRange"/>
        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes a range of elements.
        /// </summary>
        /// <param name="range">
        /// The range of elements to remove.
        /// </param>
        /// <seealso cref="Remove"/>
        /// <seealso cref="RemoveAt"/>
        public void RemoveRange(Range range)
        {
            (int start, int count) = range.GetOffsetAndLength(this.Count);
            this.RemoveRange(start, count);
        }

        /// <summary>
        /// Removes a range of elements.
        /// </summary>
        /// <param name="start">
        /// The zero-based starting index of the range of elements to remove.
        /// </param>
        /// <param name="count">
        /// The number of elements to remove.
        /// </param>
        /// <seealso cref="Remove"/>
        /// <seealso cref="RemoveAt"/>
        public void RemoveRange(int start, int count) => throw new NotImplementedException();

        [DoesNotReturn]
        private void ThrowObjectDisposedException() => throw new ObjectDisposedException(this.GetType().Name);

        public struct Enumerator : IEnumerator<T>
        {
            private readonly Array<T> array;
            private readonly int count;
            private int index;
            private T current;

            internal Enumerator(Array<T> array)
            {
                this.array = array;
                this.count = array.Count;
                this.index = 0;
                this.current = default;
            }

            public T Current => this.current;

            object? IEnumerator.Current => (uint)this.index > (uint)this.array.Count ? throw new InvalidOperationException() : this.Current;

            public void Dispose() { }

            /// <exception cref="InvalidOperationException"></exception>
            public bool MoveNext()
            {
                if (this.count != this.array.Count)
                {
                    Throw.InvalidOperationException();
                }

                if ((uint)this.index < (uint)this.array.Count)
                {
                    this.current = this.array[this.index];
                    this.index++;
                    return true;
                }

                this.index = this.array.Count + 1;
                this.current = default;
                return false;
            }

            public void Reset()
            {
                this.index = 0;
                this.current = default;
            }
        }
    }
}
