// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

// using System;
// using NextTurn.UE.Processors;

// namespace Unreal
// {
//     internal struct CppStruct
//     {
//         private readonly IntPtr pointer;

//         internal CppStruct(IntPtr pointer) => this.pointer = pointer;

//         public bool IsTriviallyDefaultConstructible => NativeMethods.IsTriviallyDefaultConstructible(this.pointer);

//         public bool IsTriviallyDestructible => NativeMethods.IsTriviallyDestructible(this.pointer);

//         public int Size => NativeMethods.GetSize(this.pointer);

//         private static class NativeMethods
//         {
//             [ReadOffset]
//             public static extern int GetSize(IntPtr cppStruct);

//             [Calli]
//             public static extern bool IsTriviallyDefaultConstructible(IntPtr cppStruct);

//             [Calli]
//             public static extern bool IsTriviallyDestructible(IntPtr cppStruct);
//         }
//     }
// }
