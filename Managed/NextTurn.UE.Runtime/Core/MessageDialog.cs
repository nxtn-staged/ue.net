// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using NextTurn.UE.Annotations;

namespace Unreal
{
    /// <summary>
    /// Displays a message dialog.
    /// </summary>
    public static class MessageDialog
    {
        /// <summary>
        /// Displays a message dialog with the specified content and buttons.
        /// </summary>
        /// <param name="content">
        /// The content to display.
        /// </param>
        /// <param name="button">
        /// The button or buttons to display.
        /// </param>
        /// <returns>
        /// The message dialog button clicked by the user.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="content"/> is null.
        /// </exception>
        public static unsafe Result Show(Text content, Button button)
        {
            if (content is null)
            {
                ThrowContentNullException();
            }

            return NativeMethods.Show(content.text, Unsafe.AsRef<NativeText>(null), button);
        }

        /// <summary>
        /// Displays a message dialog with the specified content, buttons and default result.
        /// </summary>
        /// <param name="content">
        /// The content to display.
        /// </param>
        /// <param name="button">
        /// The button or buttons to display.
        /// </param>
        /// <param name="defaultResult">
        /// The default result if the error reporting mode is unattended.
        /// </param>
        /// <returns>
        /// The message dialog button clicked by the user, or <paramref name="defaultResult"/> if the error reporting mode is unattended.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="content"/> is null.
        /// </exception>
        public static unsafe Result Show(Text content, Button button, Result defaultResult)
        {
            if (content is null)
            {
                ThrowContentNullException();
            }

            return NativeMethods.ShowWithDefaultResult(content.text, Unsafe.AsRef<NativeText>(null), button, defaultResult);
        }

        /// <summary>
        /// Displays a message dialog with the specified content, title and buttons.
        /// </summary>
        /// <param name="content">
        /// The content to display.
        /// </param>
        /// <param name="title">
        /// The title to display.
        /// </param>
        /// <param name="button">
        /// The button or buttons to display.
        /// </param>
        /// <returns>
        /// The message dialog button clicked by the user.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="content"/> or <paramref name="title"/> is null.
        /// </exception>
        public static Result Show(Text content, Text title, Button button)
        {
            if (content is null)
            {
                ThrowContentNullException();
            }

            if (title is null)
            {
                ThrowTitleNullException();
            }

            return NativeMethods.Show(content.text, title.text, button);
        }

        /// <summary>
        /// Displays a message dialog with the specified content, title, buttons and default result.
        /// </summary>
        /// <param name="content">
        /// The content to display.
        /// </param>
        /// <param name="title">
        /// The title to display.
        /// </param>
        /// <param name="button">
        /// The button or buttons to display.
        /// </param>
        /// <param name="defaultResult">
        /// The default result if the error reporting mode is unattended.
        /// </param>
        /// <returns>
        /// The message dialog button clicked by the user, or <paramref name="defaultResult"/> if the error reporting mode is unattended.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="content"/> or <paramref name="title"/> is null.
        /// </exception>
        public static Result Show(Text content, Text title, Button button, Result defaultResult)
        {
            if (content is null)
            {
                ThrowContentNullException();
            }

            if (title is null)
            {
                ThrowTitleNullException();
            }

            return NativeMethods.ShowWithDefaultResult(content.text, title.text, button, defaultResult);
        }

        [DoesNotReturn]
        private static void ThrowContentNullException() => throw new ArgumentNullException("content");

        [DoesNotReturn]
        private static void ThrowTitleNullException() => throw new ArgumentNullException("title");

        /// <summary>
        /// Specifies the buttons displayed on a message dialog.
        /// </summary>
        public enum Button
        {
            /// <summary>
            /// The message dialog displays an <b>OK</b> button.
            /// </summary>
            Ok,

            /// <summary>
            /// The message dialog displays <b>Yes</b> and <b>No</b> buttons.
            /// </summary>
            YesNo,

            /// <summary>
            /// The message dialog displays <b>OK</b> and <b>Cancel</b> buttons.
            /// </summary>
            OkCancel,

            /// <summary>
            /// The message dialog displays <b>Yes</b>, <b>No</b> and <b>Cancel</b> buttons.
            /// </summary>
            YesNoCancel,

            /// <summary>
            /// The message dialog displays <b>Cancel</b>, <b>Try Again</b> and <b>Continue</b> buttons.
            /// </summary>
            CancelRetryContinue,

            /// <summary>
            /// The message dialog displays <b>Yes</b>, <b>No</b>, <b>Yes to All</b> and <b>No to All</b> buttons.
            /// </summary>
            YesNoYesAllNoAll,

            /// <summary>
            /// The message dialog displays <b>Yes</b>, <b>No</b>, <b>Yes to All</b>, <b>No to All</b> and <b>Cancel</b> buttons.
            /// </summary>
            YesNoYesAllNoAllCancel,

            /// <summary>
            /// The message dialog displays <b>Yes</b>, <b>No</b> and <b>Yes to All</b> buttons.
            /// </summary>
            YesNoYesAll,
        }

        /// <summary>
        /// Specifies the button clicked by the user.
        /// </summary>
        public enum Result
        {
            /// <summary>
            /// The result value of the message dialog is <b>No</b>.
            /// </summary>
            No,

            /// <summary>
            /// The result value of the message dialog is <b>Yes</b>.
            /// </summary>
            Yes,

            /// <summary>
            /// The result value of the message dialog is <b>Yes to All</b>.
            /// </summary>
            YesAll,

            /// <summary>
            /// The result value of the message dialog is <b>No to All</b>.
            /// </summary>
            NoAll,

            /// <summary>
            /// The result value of the message dialog is <b>Cancel</b>.
            /// </summary>
            Cancel,

            /// <summary>
            /// The result value of the message dialog is <b>OK</b>.
            /// </summary>
            Ok,

            /// <summary>
            /// The result value of the message dialog is <b>Try Again</b>.
            /// </summary>
            Retry,

            /// <summary>
            /// The result value of the message dialog is <b>Continue</b>.
            /// </summary>
            Continue,
        }

        private static class NativeMethods
        {
            [Calli]
            public static extern Result Show(in NativeText content, in NativeText title, Button button);

            [Calli]
            public static extern Result ShowWithDefaultResult(in NativeText content, in NativeText title, Button button, Result defaultResult);
        }
    }
}
