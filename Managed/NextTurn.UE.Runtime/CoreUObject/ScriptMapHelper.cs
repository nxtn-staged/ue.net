// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Unreal
{
    internal readonly struct ScriptMapHelper
    {
        private readonly IntPtr keyProperty;
        private readonly IntPtr valueProperty;
        private readonly unsafe ScriptMap* map;
        private readonly ScriptMap.Layout layout;

        internal unsafe ScriptMapHelper(MapProperty property, IntPtr map)
        {
            this.keyProperty = MapProperty.NativeMethods.GetKeyProperty(property.pointer);
            this.valueProperty = MapProperty.NativeMethods.GetValueProperty(property.pointer);
            this.map = (ScriptMap*)map;
            this.layout = MapProperty.NativeMethods.GetLayout(property.pointer);
        }

        internal unsafe int Count => this.map->Count;

        internal unsafe void Add(IntPtr key, IntPtr value)
        {
            IntPtr localkeyProperty = this.keyProperty;
            IntPtr localValueProperty = this.valueProperty;
            this.map->Add(
                key,
                this.layout,
                key => Property.UnsafeMethods.GetNativeHashCode(localkeyProperty, key),
                (left, right) => Property.UnsafeMethods.Equals(localkeyProperty, left, right),
                key => Property.UnsafeMethods.InitializeValue(localkeyProperty, key),
                value => Property.UnsafeMethods.InitializeValue(localValueProperty, value),
                key => Property.UnsafeMethods.FinalizeValue(localkeyProperty, key),
                value => Property.UnsafeMethods.FinalizeValue(localValueProperty, value));
        }

        internal unsafe void Add<TKey, TValue>(TKey key, TValue value)
        {
            byte* keyBuffer = stackalloc byte[Property.NativeMethods.GetElementSize(this.keyProperty)];
            byte* valueBuffer = stackalloc byte[Property.NativeMethods.GetElementSize(this.valueProperty)];
            NextTurn.UE.Marshaler<TKey>.ToNative(new IntPtr(keyBuffer), 0, key);
            NextTurn.UE.Marshaler<TValue>.ToNative(new IntPtr(valueBuffer), 0, value);
            this.Add(new IntPtr(keyBuffer), new IntPtr(valueBuffer));
        }

        internal unsafe void Clear()
        {
            int oldCount = this.Count;
            if (oldCount != 0)
            {
                this.Finalize(0, oldCount);
                this.map->Clear(this.layout);
            }
        }

        internal unsafe bool Contains<TKey, TValue>(TKey key, TValue value)
        {
            IntPtr valuePtr = this.FindValuePtr(key);
            if (valuePtr != IntPtr.Zero)
            {
                byte* valueBuffer = stackalloc byte[Property.NativeMethods.GetElementSize(this.valueProperty)];
                NextTurn.UE.Marshaler<TValue>.ToNative(new IntPtr(valueBuffer), 0, value);
                return Property.UnsafeMethods.Equals(this.valueProperty, valuePtr, new IntPtr(valueBuffer));
            }

            return false;
        }

        internal unsafe bool ContainsKey<TKey>(TKey key)
        {
            byte* keyBuffer = stackalloc byte[Property.NativeMethods.GetElementSize(this.keyProperty)];
            NextTurn.UE.Marshaler<TKey>.ToNative(new IntPtr(keyBuffer), 0, key);
            return this.IndexOf(new IntPtr(keyBuffer)) >= 0;
        }

        private unsafe IntPtr FindValuePtr(IntPtr key)
        {
            IntPtr localkeyProperty = this.keyProperty;
            return this.map->FindValuePtr(
                key,
                this.layout,
                key => Property.UnsafeMethods.GetNativeHashCode(localkeyProperty, key),
                (left, right) => Property.UnsafeMethods.Equals(localkeyProperty, left, right));
        }

        internal unsafe IntPtr FindValuePtr<TKey>(TKey key)
        {
            byte* keyBuffer = stackalloc byte[Property.NativeMethods.GetElementSize(this.keyProperty)];
            NextTurn.UE.Marshaler<TKey>.ToNative(new IntPtr(keyBuffer), 0, key);
            return this.FindValuePtr(new IntPtr(keyBuffer));
        }

        private unsafe int IndexOf(IntPtr key)
        {
            IntPtr localkeyProperty = this.keyProperty;
            return this.map->IndexOf(
                key,
                this.layout,
                key => Property.UnsafeMethods.GetNativeHashCode(localkeyProperty, key),
                (left, right) => Property.UnsafeMethods.Equals(localkeyProperty, left, right));
        }

        private void Finalize(int index, int count)
        {
            throw new NotImplementedException();
        }

        private bool Remove(IntPtr key)
        {
            int index = this.IndexOf(key);
            if (index >= 0)
            {
                this.RemoveAt(index);
                return true;
            }

            return false;
        }

        internal unsafe bool Remove<TKey>(TKey key)
        {
            byte* keyBuffer = stackalloc byte[Property.NativeMethods.GetElementSize(this.keyProperty)];
            NextTurn.UE.Marshaler<TKey>.ToNative(new IntPtr(keyBuffer), 0, key);
            return this.Remove(new IntPtr(keyBuffer));
        }

        internal unsafe bool Remove<TKey, TValue>(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            byte* keyBuffer = stackalloc byte[Property.NativeMethods.GetElementSize(this.keyProperty)];
            NextTurn.UE.Marshaler<TKey>.ToNative(new IntPtr(keyBuffer), 0, key);

            int index = this.IndexOf(new IntPtr(keyBuffer));
            if (index >= 0)
            {
                value = NextTurn.UE.Marshaler<TValue>.ToManaged(this.map->GetValuePtr(index, this.layout), 0);
                this.RemoveAt(index);
                return true;
            }

            value = default!;
            return false;
        }

        internal unsafe bool Remove<TKey, TValue>(KeyValuePair<TKey, TValue> item)
        {
            byte* keyBuffer = stackalloc byte[Property.NativeMethods.GetElementSize(this.keyProperty)];
            NextTurn.UE.Marshaler<TKey>.ToNative(new IntPtr(keyBuffer), 0, item.Key);

            int index = this.IndexOf(new IntPtr(keyBuffer));
            if (index >= 0)
            {
                IntPtr valuePtr = this.map->GetValuePtr(index, this.layout);
                byte* valueBuffer = stackalloc byte[Property.NativeMethods.GetElementSize(this.valueProperty)];
                NextTurn.UE.Marshaler<TValue>.ToNative(new IntPtr(valueBuffer), 0, item.Value);
                if (Property.UnsafeMethods.Equals(this.valueProperty, valuePtr, new IntPtr(valueBuffer)))
                {
                    this.RemoveAt(index);
                    return true;
                }
            }

            return false;
        }

        internal unsafe void RemoveAt(int index, int count = 1)
        {
            this.Finalize(index, count);
            throw new NotImplementedException();
        }
    }
}
