// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using NextTurn.UE.Annotations;

namespace Unreal.Slate
{
    public sealed class BindingContext :
#nullable disable // to enable use with both T and T? for reference types due to IEquatable<T> being invariant
        IEquatable<BindingContext>
#nullable restore
    {
        internal SharedReference reference;

        internal BindingContext() { }

        /// <param name="name">
        /// The <see cref="Unreal.Name"/> of the <see cref="BindingContext"/>.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="styleSet"/> is <see cref="Name.None"/>.
        /// </exception>
        public BindingContext(Name name, Text description, Name parent, Name styleSet)
        {
            if (styleSet.IsNone)
            {
                throw new ArgumentException(nameof(styleSet));
            }

            NativeMethods.Initialize(out this.reference, name, description.text, parent, styleSet);
        }

        public Text Description => new Text(this.reference.GetTarget<NativeBindingContext>().Description);

        public Name Name => this.reference.GetTarget<NativeBindingContext>().Name;

        public Name ParentName => this.reference.GetTarget<NativeBindingContext>().ParentName;

        public Name StyleSetName => this.reference.GetTarget<NativeBindingContext>().StyleSetName;

        public override bool Equals(object? value) => value is BindingContext other && this.Equals(other);

        public bool Equals(BindingContext? other) =>
            other is null ? false :
            ReferenceEquals(this, other) ? true :
            this.Name.Equals(other.Name);

        public override int GetHashCode() => this.Name.GetHashCode();

        public static bool operator ==(BindingContext? left, BindingContext? right) =>
            right is null ? (left is null) :
            ReferenceEquals(left, right) ? true :
            right.Equals(left);

        public static bool operator !=(BindingContext? left, BindingContext? right) => !(left == right);

        internal readonly struct NativeBindingContext
        {
            public NativeBindingContext(Name name, NativeText description, Name parentName, Name styleSetName)
            {
                this.Name = name;
                this.Description = description;
                this.ParentName = parentName;
                this.StyleSetName = styleSetName;
            }

            internal Name Name { get; }

            internal Name ParentName { get; }

            internal NativeText Description { get; }

            internal Name StyleSetName { get; }
        }

        private static class NativeMethods
        {
            [Calli]
            public static extern void Initialize(out SharedReference context, Name name, in NativeText description, Name parent, Name styleSet);
        }
    }
}
