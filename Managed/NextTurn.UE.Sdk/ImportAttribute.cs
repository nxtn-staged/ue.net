// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;

namespace NextTurn.UE
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ImportAttribute : Attribute
    {
        public ImportAttribute(ImportOperation value) => this.Value = value;

        public ImportOperation Value { get; }
    }
}
