// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using NextTurn.UE;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public static class Internationalization
    {
        public static unsafe Culture CurrentCulture =>
            new Culture(NativeMethods.GetCurrentCulture());

        public static unsafe Culture CurrentLanguage =>
            new Culture(NativeMethods.GetCurrentLanguage());

        public static unsafe Culture CurrentLocale =>
            new Culture(NativeMethods.GetCurrentLocale());

        public static unsafe Culture DefaultCulture =>
            new Culture(NativeMethods.GetDefaultCulture());

        public static unsafe Culture DefaultLanguage =>
            new Culture(NativeMethods.GetDefaultLanguage());

        public static unsafe Culture DefaultLocale =>
            new Culture(NativeMethods.GetDefaultLocale());

        public static unsafe Culture InvariantCulture =>
            new Culture(NativeMethods.GetInvariantCulture());

        public static unsafe bool SetCurrentCulture(string cultureName)
        {
            ScriptArray nativeCultureName;
            StringMarshaler.ToNative(&nativeCultureName, cultureName);
            return NativeMethods.SetCurrentCulture(&nativeCultureName);
        }

        public static unsafe bool SetCurrentLanguage(string cultureName)
        {
            ScriptArray nativeCultureName;
            StringMarshaler.ToNative(&nativeCultureName, cultureName);
            return NativeMethods.SetCurrentLanguage(&nativeCultureName);
        }

        public static unsafe bool SetCurrentLocale(string cultureName)
        {
            ScriptArray nativeCultureName;
            StringMarshaler.ToNative(&nativeCultureName, cultureName);
            return NativeMethods.SetCurrentLocale(&nativeCultureName);
        }

        private static class NativeMethods
        {
            [Calli]
            public static extern unsafe SharedReference* GetCurrentCulture();

            [Calli]
            public static extern unsafe SharedReference* GetCurrentLanguage();

            [Calli]
            public static extern unsafe SharedReference* GetCurrentLocale();

            [Calli]
            public static extern unsafe SharedReference* GetDefaultCulture();

            [Calli]
            public static extern unsafe SharedReference* GetDefaultLanguage();

            [Calli]
            public static extern unsafe SharedReference* GetDefaultLocale();

            [Calli]
            public static extern unsafe SharedReference* GetInvariantCulture();

            [Calli]
            public static extern unsafe bool SetCurrentCulture(ScriptArray* cultureName);

            [Calli]
            public static extern unsafe bool SetCurrentLanguage(ScriptArray* cultureName);

            [Calli]
            public static extern unsafe bool SetCurrentLocale(ScriptArray* cultureName);
        }
    }
}
