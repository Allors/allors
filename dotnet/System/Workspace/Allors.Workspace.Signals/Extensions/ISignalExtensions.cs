// <copyright file="Object.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Signals
{
    public static class ISignalExtensions
    {
        public static void Set<T>(this ISignal<IUnitRole<T>?> signal, T value, T defaultValue = default) where T : struct
        {
            if (signal is { Value: not null })
            {
                signal.Value.Value = value;
            }
        }

        public static void Set<T>(this ISignal<IUnitRole<T?>?> signal, T? value) where T : struct
        {
            if (signal is { Value: not null })
            {
                signal.Value.Value = value;
            }
        }

        public static void Set<T>(this ISignal<IUnitRole<T>> signal, T value) where T : class
        {
            if (signal is { Value: not null })
            {
                signal.Value.Value = value;
            }
        }
    }
}
