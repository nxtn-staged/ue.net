// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

namespace Unreal
{
    public interface IProperty<T>
    {
        /// <summary>
        /// Returns the property value of a specified <see cref="Object"/> with an optional index value for indexed properties.
        /// </summary>
        /// <param name="object">
        /// The <see cref="Object"/> whose property value will be returned.
        /// </param>
        /// <param name="index">
        /// An optional index value for indexed properties.
        /// </param>
        /// <returns>
        /// The property value of the specified <see cref="Object"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="object"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0.
        /// -or-
        /// <paramref name="index"/> is greater than or equal to <see cref="Property.ArrayLength"/>.
        /// </exception>
        T GetValue(Object @object, int index = 0);

        /// <summary>
        /// Sets the property value of a specified <see cref="Object"/> with an optional index value for indexed properties.
        /// </summary>
        /// <param name="object">
        /// The <see cref="Object"/> whose property value will be set.
        /// </param>
        /// <param name="value">
        /// The new property value.
        /// </param>
        /// <param name="index">
        /// An optional index value for indexed properties.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="object"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0.
        /// -or-
        /// <paramref name="index"/> is greater than or equal to <see cref="Property.ArrayLength"/>.
        /// </exception>
        void SetValue(Object @object, T value, int index = 0);
    }
}
