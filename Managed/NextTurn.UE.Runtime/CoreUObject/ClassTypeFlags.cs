// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    [Flags]
    public enum ClassTypeFlags : long
    {
        None = 0x0000_0000_0000_0000,

        /// <summary>
        /// The <see cref="Unreal.Member"/> type.
        /// </summary>
        Member = 0x0000_0000_0000_0001,

        /// <summary>
        /// The <see cref="Unreal.SByteProperty"/> type.
        /// </summary>
        SByteProperty = 0x0000_0000_0000_0002,

        /// <summary>
        /// The <see cref="Unreal.Enum"/> type.
        /// </summary>
        Enum = 0x0000_0000_0000_0004,

        /// <summary>
        /// The <see cref="Unreal.CompoundMember"/> type.
        /// </summary>
        CompoundMember = 0x0000_0000_0000_0008,

        /// <summary>
        /// The <see cref="Unreal.Struct"/> type.
        /// </summary>
        Struct = 0x0000_0000_0000_0010,

        /// <summary>
        /// The <see cref="Unreal.Class"/> type.
        /// </summary>
        Class = 0x0000_0000_0000_0020,

        /// <summary>
        /// The <see cref="Unreal.ByteProperty"/> type.
        /// </summary>
        ByteProperty = 0x0000_0000_0000_0040,

        /// <summary>
        /// The <see cref="Unreal.Int32Property"/> type.
        /// </summary>
        Int32Property = 0x0000_0000_0000_0080,

        /// <summary>
        /// The <see cref="Unreal.SingleProperty"/> type.
        /// </summary>
        SingleProperty = 0x0000_0000_0000_0100,

        /// <summary>
        /// The <see cref="Unreal.UInt64Property"/> type.
        /// </summary>
        UInt64Property = 0x0000_0000_0000_0200,

        /// <summary>
        /// The <see cref="Unreal.ClassProperty"/> type.
        /// </summary>
        ClassProperty = 0x0000_0000_0000_0400,

        /// <summary>
        /// The <see cref="Unreal.UInt32Property"/> type.
        /// </summary>
        UInt32Property = 0x0000_0000_0000_0800,

        /// <summary>
        /// The <see cref="Unreal.InterfaceProperty"/> type.
        /// </summary>
        InterfaceProperty = 0x0000_0000_0000_1000,

        /// <summary>
        /// The <see cref="Unreal.NameProperty"/> type.
        /// </summary>
        NameProperty = 0x0000_0000_0000_2000,

        /// <summary>
        /// The <see cref="Unreal.StringProperty"/> type.
        /// </summary>
        StringProperty = 0x0000_0000_0000_4000,

        /// <summary>
        /// The <see cref="Unreal.Property"/> type.
        /// </summary>
        Property = 0x0000_0000_0000_8000,

        /// <summary>
        /// The <see cref="Unreal.ObjectProperty"/> type.
        /// </summary>
        ObjectProperty = 0x0000_0000_0001_0000,

        /// <summary>
        /// The <see cref="Unreal.BooleanProperty"/> type.
        /// </summary>
        BooleanProperty = 0x0000_0000_0002_0000,

        /// <summary>
        /// The <see cref="Unreal.UInt16Property"/> type.
        /// </summary>
        UInt16Property = 0x0000_0000_0004_0000,

        /// <summary>
        /// The <see cref="Unreal.Method"/> type.
        /// </summary>
        Method = 0x0000_0000_0008_0000,

        /// <summary>
        /// The <see cref="Unreal.StructProperty"/> type.
        /// </summary>
        StructProperty = 0x0000_0000_0010_0000,

        /// <summary>
        /// The <see cref="Unreal.ArrayProperty"/> type.
        /// </summary>
        ArrayProperty = 0x0000_0000_0020_0000,

        /// <summary>
        /// The <see cref="Unreal.Int64Property"/> type.
        /// </summary>
        Int64Property = 0x0000_0000_0040_0000,

        /// <summary>
        /// The <see cref="Unreal.DelegateProperty"/> type.
        /// </summary>
        DelegateProperty = 0x0000_0000_0080_0000,

        /// <summary>
        /// The <see cref="Unreal.NumericProperty"/> type.
        /// </summary>
        NumericProperty = 0x0000_0000_0100_0000,

        /// <summary>
        /// The <see cref="Unreal.MulticastDelegateProperty"/> type.
        /// </summary>
        MulticastDelegateProperty = 0x0000_0000_0200_0000,

        /// <summary>
        /// The <see cref="Unreal.ObjectPropertyBase"/> type.
        /// </summary>
        ObjectPropertyBase = 0x0000_0000_0400_0000,

        /// <summary>
        /// The <see cref="Unreal.WeakObjectProperty"/> type.
        /// </summary>
        WeakObjectProperty = 0x0000_0000_0800_0000,

        /// <summary>
        /// The <see cref="Unreal.LazyObjectProperty"/> type.
        /// </summary>
        LazyObjectProperty = 0x0000_0000_1000_0000,

        /// <summary>
        /// The <see cref="Unreal.SoftObjectProperty"/> type.
        /// </summary>
        SoftObjectProperty = 0x0000_0000_2000_0000,

        /// <summary>
        /// The <see cref="Unreal.TextProperty"/> type.
        /// </summary>
        TextProperty = 0x0000_0000_4000_0000,

        /// <summary>
        /// The <see cref="Unreal.Int16Property"/> type.
        /// </summary>
        Int16Property = 0x0000_0000_8000_0000,

        /// <summary>
        /// The <see cref="Unreal.DoubleProperty"/> type.
        /// </summary>
        DoubleProperty = 0x0000_0001_0000_0000,

        /// <summary>
        /// The <see cref="Unreal.SoftClassProperty"/> type.
        /// </summary>
        SoftClassProperty = 0x0000_0002_0000_0000,

        /// <summary>
        /// The <see cref="Unreal.Package"/> type.
        /// </summary>
        Package = 0x0000_0004_0000_0000,

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
        DelegateMethod = 0x0000_1000_0000_0000,

        //UStaticMeshComponent = 0x0200000000000,

        /// <summary>
        /// The <see cref="Unreal.MapProperty"/> type.
        /// </summary>
        MapProperty = 0x0000_4000_0000_0000,

        /// <summary>
        /// The <see cref="Unreal.SetProperty"/> type.
        /// </summary>
        SetProperty = 0x0000_8000_0000_0000,

        /// <summary>
        /// The <see cref="Unreal.EnumProperty"/> type.
        /// </summary>
        EnumProperty = 0x0001_0000_0000_0000,

        /// <summary>
        /// The <see cref="Unreal.SparseDelegateMethod"/> type.
        /// </summary>
        SparseDelegateMethod = 0x0002_0000_0000_0000,

        /// <summary>
        /// The <see cref="Unreal.MulticastInlineDelegateProperty"/> type.
        /// </summary>
        MulticastInlineDelegateProperty = 0x0004_0000_0000_0000,

        /// <summary>
        /// The <see cref="Unreal.MulticastSparseDelegateProperty"/> type.
        /// </summary>
        MulticastSparseDelegateProperty = 0x0008_0000_0000_0000,

        /// <summary>
        /// The <see cref="Unreal.PropertyPathProperty"/> type.
        /// </summary>
        FieldPathProperty = 0x0010_0000_0000_0000,
    }
}
