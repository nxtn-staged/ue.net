// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    internal readonly struct ScriptSetHelper
    {
        private readonly IntPtr itemProperty;
        private readonly unsafe ScriptSet* set;
        private readonly ScriptSet.Layout layout;

        internal unsafe ScriptSetHelper(SetProperty property, IntPtr set)
        {
            this.itemProperty = SetProperty.NativeMethods.GetItemProperty(property.pointer);
            this.set = (ScriptSet*)set;
            this.layout = SetProperty.NativeMethods.GetLayout(property.pointer);
        }

        internal unsafe int Count => this.set->Count;

        private unsafe void Add(IntPtr item)
        {
            IntPtr localItemProperty = this.itemProperty;
            this.set->Add(
                item,
                this.layout,
                item => Property.UnsafeMethods.GetNativeHashCode(localItemProperty, item),
                (left, right) => Property.UnsafeMethods.Equals(localItemProperty, left, right),
                item => Property.UnsafeMethods.InitializeValue(localItemProperty, item),
                item => Property.UnsafeMethods.FinalizeValue(localItemProperty, item));
        }

        internal unsafe void Add<T>(T item)
        {
            byte* itemBuffer = stackalloc byte[Property.NativeMethods.GetElementSize(this.itemProperty)];
            NextTurn.UE.Marshaler<T>.ToNative(new IntPtr(itemBuffer), 0, item);
            this.Add(new IntPtr(itemBuffer));
        }

        internal unsafe void Clear()
        {
            int oldCount = this.Count;
            if (oldCount != 0)
            {
                this.Finalize(0, oldCount);
                this.set->Clear(this.layout);
            }
        }

        internal unsafe bool Contains<T>(T item)
        {
            byte* itemBuffer = stackalloc byte[Property.NativeMethods.GetElementSize(this.itemProperty)];
            NextTurn.UE.Marshaler<T>.ToNative(new IntPtr(itemBuffer), 0, item);
            return this.IndexOf(new IntPtr(itemBuffer)) >= 0;
        }

        internal unsafe IntPtr Find(IntPtr item)
        {
            int index = this.IndexOf(item);
            return index >= 0 ? this.set->GetEntryPtr(index, this.layout) : IntPtr.Zero;
        }

        private unsafe int IndexOf(IntPtr item)
        {
            IntPtr localItemProperty = this.itemProperty;
            return this.set->IndexOf(
                item,
                this.layout,
                item => Property.UnsafeMethods.GetNativeHashCode(localItemProperty, item),
                (left, right) => Property.UnsafeMethods.Equals(localItemProperty, left, right));
        }

        private void Finalize(int index, int count)
        {
        }

        internal unsafe bool Remove<T>(T item)
        {
            byte* itemBuffer = stackalloc byte[Property.NativeMethods.GetElementSize(this.itemProperty)];
            NextTurn.UE.Marshaler<T>.ToNative(new IntPtr(itemBuffer), 0, item);
            int index = this.IndexOf(new IntPtr(itemBuffer));
            if (index >= 0)
            {
                this.set->RemoveAt(index, this.layout);
                return true;
            }

            return false;
        }
    }
}
