// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Generic;
using Leftice.Processors;

namespace Unreal
{
    public sealed class Enum : Member
    {
        internal Enum(IntPtr pointer) : base(pointer) { }

        public unsafe long MaxValue
        {
            get
            {
                long max = this.NamesPtr->GetItem<KeyValuePair<Name, long>>(0).Value;
                for (int i = 1; i < this.NamesPtr->Count; i++)
                {
                    long current = this.NamesPtr->GetItem<KeyValuePair<Name, long>>(i).Value;
                    if (current > max)
                    {
                        max = current;
                    }
                }

                return max;
            }
        }

        private unsafe ScriptArray* NamesPtr => NativeMethods.GetNames(this.pointer);

        public Text GetDisplayNameTextByIndex(int index)
        {
            Text result = new Text();
            NativeMethods.GetDisplayNameTextByIndex(this.pointer, index, out result.text);
            return result;
        }

        public Text GetDisplayNameTextByValue(long value)
        {
            Text result = new Text();
            NativeMethods.GetDisplayNameTextByValue(this.pointer, value, out result.text);
            return result;
        }

        public int GetIndexByName(Name name) => NativeMethods.GetIndexByName(this.pointer, name);

        public unsafe int GetIndexByNameString(string name)
        {
            ScriptArray nativeName;
            Leftice.StringMarshaler.ToNative(&nativeName, name);
            return NativeMethods.GetIndexByNameString(this.pointer, nativeName);
        }

        public unsafe int GetIndexByValue(long value)
        {
            return this.NamesPtr->FindIndex<KeyValuePair<Name, long>>(pair => pair.Value == value);
        }

        public unsafe Name GetNameByIndex(int index)
        {
            return this.NamesPtr->GetItem<KeyValuePair<Name, long>>(index).Key;
        }

        public unsafe Name GetNameByValue(long value)
        {
            return this.NamesPtr->FirstOrDefault<KeyValuePair<Name, long>>(pair => pair.Value == value).Key;
        }

        public unsafe string GetNameStringByIndex(int index)
        {
            NativeMethods.GetNameStringByIndex(this.pointer, index, out ScriptArray nativeResult);
            return Leftice.StringMarshaler.ToManagedFinally(&nativeResult);
        }

        public string GetNameStringByValue(long value)
        {
            int index = this.GetIndexByValue(value);
            return this.GetNameStringByIndex(index);
        }

        public unsafe long GetValueByIndex(int index)
        {
            return this.NamesPtr->GetItem<KeyValuePair<Name, long>>(index).Value;
        }

        public unsafe long GetValueByName(Name name)
        {
            int index = this.GetIndexByName(name);
            return this.NamesPtr->GetItem<KeyValuePair<Name, long>>(index).Value;
        }

        public long GetValueByNameString(string name)
        {
            int index = this.GetIndexByNameString(name);
            return this.GetValueByIndex(index);
        }

        public unsafe bool IsValidName(Name name) => this.NamesPtr->Any<KeyValuePair<Name, long>>(pair => pair.Key == name);

        public unsafe bool IsValidValue(long value) => this.NamesPtr->Any<KeyValuePair<Name, long>>(pair => pair.Value == value);

        private static new class NativeMethods
        {
            [Calli]
            public static extern void GetDisplayNameTextByIndex(IntPtr @enum, int index, out NativeText text);

            [Calli]
            public static extern void GetDisplayNameTextByValue(IntPtr @enum, long value, out NativeText text);

            [Calli]
            public static extern int GetIndexByName(IntPtr @enum, Name name);

            [Calli]
            public static extern int GetIndexByNameString(IntPtr @enum, in ScriptArray nativeName);

            [Calli]
            public static extern void GetNameStringByIndex(IntPtr @enum, int index, out ScriptArray nativeResult);

            [PointerOffset]
            public static extern unsafe ScriptArray* GetNames(IntPtr @enum);
        }
    }
}
