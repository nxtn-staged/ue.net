// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using Xunit;

namespace Unreal.Editor.Tests
{
    public static class FieldExtensionsTests
    {
        private const string FieldParamName = "field";

        [Fact]
        public static void GetDisplayNameText_Invalid()
        {
            _ = Assert.Throws<ArgumentNullException>(FieldParamName, () => default(Field)!.GetDisplayNameText());
        }

        [Fact]
        public static void GetMetaData_Invalid()
        {
            _ = Assert.Throws<ArgumentNullException>(FieldParamName, () => default(Field)!.GetMetaData(default(Name)));
            _ = Assert.Throws<ArgumentNullException>(FieldParamName, () => default(Field)!.GetMetaData(default(string)));
        }

        [Fact]
        public static void GetToolTipText_Invalid()
        {
            _ = Assert.Throws<ArgumentNullException>(FieldParamName, () => default(Field)!.GetToolTipText());
        }

        [Fact]
        public static void HasMetaData_Invalid()
        {
            _ = Assert.Throws<ArgumentNullException>(FieldParamName, () => default(Field)!.HasMetaData(default(Name)));
            _ = Assert.Throws<ArgumentNullException>(FieldParamName, () => default(Field)!.HasMetaData(default(string)));
        }

        [Fact]
        public static void RemoveMetaData_Invalid()
        {
            _ = Assert.Throws<ArgumentNullException>(FieldParamName, () => default(Field)!.RemoveMetaData(default(Name)));
            _ = Assert.Throws<ArgumentNullException>(FieldParamName, () => default(Field)!.RemoveMetaData(default(string)));
        }

        [Fact]
        public static void SetMetaData_Invalid()
        {
            _ = Assert.Throws<ArgumentNullException>(FieldParamName, () => default(Field)!.SetMetaData(default(Name), default!));
            _ = Assert.Throws<ArgumentNullException>(FieldParamName, () => default(Field)!.SetMetaData(default(string)!, default!));
        }

        [Fact]
        public static void TryGetMetaData_Invalid()
        {
            _ = Assert.Throws<ArgumentNullException>(FieldParamName, () => default(Field)!.TryGetMetaData(default(Name), out var _));
            _ = Assert.Throws<ArgumentNullException>(FieldParamName, () => default(Field)!.TryGetMetaData(default, out var _));
        }
    }
}
