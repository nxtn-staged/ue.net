// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;

namespace Unreal
{
    [Flags]
    public enum ClassTypeFlags : long
    {
        None = 0x0000000000000,

        /// <summary>
        /// The <see cref="Unreal.Member"/> type.
        /// </summary>
        Member = 0x0000000000001,

        /// <summary>
        /// The <see cref="Unreal.SByteProperty"/> type.
        /// </summary>
        SByteProperty = 0x0000000000002,

        /// <summary>
        /// The <see cref="Unreal.Enum"/> type.
        /// </summary>
        Enum = 0x0000000000004,

        /// <summary>
        /// The <see cref="Unreal.CompoundMember"/> type.
        /// </summary>
        CompoundMember = 0x0000000000008,

        /// <summary>
        /// The <see cref="Unreal.Struct"/> type.
        /// </summary>
        Struct = 0x0000000000010,

        /// <summary>
        /// The <see cref="Unreal.Class"/> type.
        /// </summary>
        Class = 0x0000000000020,

        /// <summary>
        /// The <see cref="Unreal.ByteProperty"/> type.
        /// </summary>
        ByteProperty = 0x0000000000040,

        /// <summary>
        /// The <see cref="Unreal.Int32Property"/> type.
        /// </summary>
        Int32Property = 0x0000000000080,

        /// <summary>
        /// The <see cref="Unreal.SingleProperty"/> type.
        /// </summary>
        SingleProperty = 0x0000000000100,

        /// <summary>
        /// The <see cref="Unreal.UInt64Property"/> type.
        /// </summary>
        UInt64Property = 0x0000000000200,

        /// <summary>
        /// The <see cref="Unreal.ClassProperty"/> type.
        /// </summary>
        ClassProperty = 0x0000000000400,

        /// <summary>
        /// The <see cref="Unreal.UInt32Property"/> type.
        /// </summary>
        UInt32Property = 0x0000000000800,

        /// <summary>
        /// The <see cref="Unreal.InterfaceProperty"/> type.
        /// </summary>
        InterfaceProperty = 0x0000000001000,

        /// <summary>
        /// The <see cref="Unreal.NameProperty"/> type.
        /// </summary>
        NameProperty = 0x0000000002000,

        /// <summary>
        /// The <see cref="Unreal.StringProperty"/> type.
        /// </summary>
        StringProperty = 0x0000000004000,

        /// <summary>
        /// The <see cref="Unreal.Property"/> type.
        /// </summary>
        Property = 0x0000000008000,

        /// <summary>
        /// The <see cref="Unreal.ObjectProperty"/> type.
        /// </summary>
        ObjectProperty = 0x0000000010000,

        /// <summary>
        /// The <see cref="Unreal.BooleanProperty"/> type.
        /// </summary>
        BooleanProperty = 0x0000000020000,

        /// <summary>
        /// The <see cref="Unreal.UInt16Property"/> type.
        /// </summary>
        UInt16Property = 0x0000000040000,

        /// <summary>
        /// The <see cref="Unreal.Method"/> type.
        /// </summary>
        Function = 0x0000000080000,

        /// <summary>
        /// The <see cref="Unreal.StructProperty"/> type.
        /// </summary>
        StructProperty = 0x0000000100000,

        /// <summary>
        /// The <see cref="Unreal.ArrayProperty"/> type.
        /// </summary>
        ArrayProperty = 0x0000000200000,

        /// <summary>
        /// The <see cref="Unreal.Int64Property"/> type.
        /// </summary>
        Int64Property = 0x0000000400000,

        /// <summary>
        /// The <see cref="Unreal.DelegateProperty"/> type.
        /// </summary>
        DelegateProperty = 0x0000000800000,

        /// <summary>
        /// The <see cref="Unreal.NumericProperty"/> type.
        /// </summary>
        NumericProperty = 0x0000001000000,

        /// <summary>
        /// The <see cref="Unreal.MulticastDelegateProperty"/> type.
        /// </summary>
        MulticastDelegateProperty = 0x0000002000000,

        /// <summary>
        /// The <see cref="Unreal.ObjectPropertyBase"/> type.
        /// </summary>
        ObjectPropertyBase = 0x0000004000000,

        /// <summary>
        /// The <see cref="Unreal.WeakObjectProperty"/> type.
        /// </summary>
        WeakObjectProperty = 0x0000008000000,

        /// <summary>
        /// The <see cref="Unreal.LazyObjectProperty"/> type.
        /// </summary>
        LazyObjectProperty = 0x0000010000000,

        /// <summary>
        /// The <see cref="Unreal.SoftObjectProperty"/> type.
        /// </summary>
        SoftObjectProperty = 0x0000020000000,

        /// <summary>
        /// The <see cref="Unreal.TextProperty"/> type.
        /// </summary>
        TextProperty = 0x0000040000000,

        /// <summary>
        /// The <see cref="Unreal.Int16Property"/> type.
        /// </summary>
        Int16Property = 0x0000080000000,

        /// <summary>
        /// The <see cref="Unreal.DoubleProperty"/> type.
        /// </summary>
        DoubleProperty = 0x0000100000000,

        /// <summary>
        /// The <see cref="Unreal.SoftClassProperty"/> type.
        /// </summary>
        SoftClassProperty = 0x0000200000000,

        /// <summary>
        /// The <see cref="Unreal.Package"/> type.
        /// </summary>
        Package = 0x0000400000000,

        //ULevel = 0x0000800000000,

        //AActor = 0x0001000000000,

        //APlayerController = 0x0002000000000,

        //APawn = 0x0004000000000,

        //USceneComponent = 0x0008000000000,

        //UPrimitiveComponent = 0x0010000000000,

        //USkinnedMeshComponent = 0x0020000000000,

        //USkeletalMeshComponent = 0x0040000000000,

        //UBlueprint = 0x0080000000000,

        /// <summary>
        /// The <see cref="Unreal.DelegateMethod"/> type.
        /// </summary>
        DelegateMethod = 0x0100000000000,

        //UStaticMeshComponent = 0x0200000000000,

        /// <summary>
        /// The <see cref="Unreal.MapProperty"/> type.
        /// </summary>
        MapProperty = 0x0400000000000,

        /// <summary>
        /// The <see cref="Unreal.SetProperty"/> type.
        /// </summary>
        SetProperty = 0x0800000000000,

        /// <summary>
        /// The <see cref="Unreal.EnumProperty"/> type.
        /// </summary>
        EnumProperty = 0x1000000000000,

        /// <summary>
        /// The <see cref="Unreal.SparseDelegateMethod"/> type.
        /// </summary>
        SparseDelegateFunction = 0x2000000000000,

        /// <summary>
        /// The <see cref="Unreal.MulticastInlineDelegateProperty"/> type.
        /// </summary>
        MulticastInlineDelegateProperty = 0x4000000000000,

        /// <summary>
        /// The <see cref="Unreal.MulticastSparseDelegateProperty"/> type.
        /// </summary>
        MulticastSparseDelegateProperty = 0x8000000000000,
    }
}
