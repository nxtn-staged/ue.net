// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using NextTurn.UE;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public static class Paths
    {
        public static unsafe string EngineDirectory
        {
            get
            {
                ScriptArray nativeResult;
                NativeMethods.GetEngineDirectory(&nativeResult);
                return StringMarshaler.ToManagedFinally(&nativeResult);
            }
        }

        public static unsafe string EngineConfigDirectory
        {
            get
            {
                ScriptArray nativeResult;
                NativeMethods.GetEngineConfigDirectory(&nativeResult);
                return StringMarshaler.ToManagedFinally(&nativeResult);
            }
        }

        public static unsafe string EngineContentDirectory
        {
            get
            {
                ScriptArray nativeResult;
                NativeMethods.GetEngineContentDirectory(&nativeResult);
                return StringMarshaler.ToManagedFinally(&nativeResult);
            }
        }

        public static unsafe string EnginePluginsDirectory
        {
            get
            {
                ScriptArray nativeResult;
                NativeMethods.GetEnginePluginsDirectory(&nativeResult);
                return StringMarshaler.ToManagedFinally(&nativeResult);
            }
        }

        public static unsafe string EngineSavedDirectory
        {
            get
            {
                ScriptArray nativeResult;
                NativeMethods.GetEngineSavedDirectory(&nativeResult);
                return StringMarshaler.ToManagedFinally(&nativeResult);
            }
        }

        public static unsafe string ProjectDirectory
        {
            get
            {
                ScriptArray nativeResult;
                NativeMethods.GetProjectDirectory(&nativeResult);
                return StringMarshaler.ToManagedFinally(&nativeResult);
            }
        }

        public static unsafe string ProjectConfigDirectory
        {
            get
            {
                ScriptArray nativeResult;
                NativeMethods.GetProjectConfigDirectory(&nativeResult);
                return StringMarshaler.ToManagedFinally(&nativeResult);
            }
        }

        public static unsafe string ProjectContentDirectory
        {
            get
            {
                ScriptArray nativeResult;
                NativeMethods.GetProjectContentDirectory(&nativeResult);
                return StringMarshaler.ToManagedFinally(&nativeResult);
            }
        }

        public static unsafe string ProjectPluginsDirectory
        {
            get
            {
                ScriptArray nativeResult;
                NativeMethods.GetProjectPluginsDirectory(&nativeResult);
                return StringMarshaler.ToManagedFinally(&nativeResult);
            }
        }

        public static unsafe string ProjectSavedDirectory
        {
            get
            {
                ScriptArray nativeResult;
                NativeMethods.GetProjectSavedDirectory(&nativeResult);
                return StringMarshaler.ToManagedFinally(&nativeResult);
            }
        }

        private static class NativeMethods
        {
            [Calli]
            public static extern unsafe void GetEngineDirectory(ScriptArray* nativeResult);

            [Calli]
            public static extern unsafe void GetEngineConfigDirectory(ScriptArray* nativeResult);

            [Calli]
            public static extern unsafe void GetEngineContentDirectory(ScriptArray* nativeResult);

            [Calli]
            public static extern unsafe void GetEnginePluginsDirectory(ScriptArray* nativeResult);

            [Calli]
            public static extern unsafe void GetEngineSavedDirectory(ScriptArray* nativeResult);

            [Calli]
            public static extern unsafe void GetProjectDirectory(ScriptArray* nativeResult);

            [Calli]
            public static extern unsafe void GetProjectConfigDirectory(ScriptArray* nativeResult);

            [Calli]
            public static extern unsafe void GetProjectContentDirectory(ScriptArray* nativeResult);

            [Calli]
            public static extern unsafe void GetProjectPluginsDirectory(ScriptArray* nativeResult);

            [Calli]
            public static extern unsafe void GetProjectSavedDirectory(ScriptArray* nativeResult);
        }
    }
}
