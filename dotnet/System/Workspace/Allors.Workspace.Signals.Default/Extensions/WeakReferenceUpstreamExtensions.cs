namespace Allors.Workspace.Signals.Default;

using System;
using System.Collections.Generic;
using System.Linq;

public static class WeakReferenceUpstreamExtensions
{
    public static WeakReference<IUpstream>[] Update(this WeakReference<IUpstream>[] @this, IUpstream newUpstream)
    {
        bool hasMissingTargets = false;
        bool hasUpstream = false;

        foreach (var weakReference in @this)
        {
            if (!weakReference.TryGetTarget(out var upstream))
            {
                hasMissingTargets = true;
                break;
            }

            if (upstream == newUpstream)
            {
                hasUpstream = true;
            }
        }

        if (hasMissingTargets == false && hasUpstream)
        {
            return @this;
        }

        IEnumerable<WeakReference<IUpstream>> result = @this;

        if (hasMissingTargets)
        {
            result = result.Where(v => v.TryGetTarget(out var upstream));
        }

        if (!hasUpstream)
        {
            result = result.Append(new WeakReference<IUpstream>(newUpstream));
        }

        return result.ToArray();
    }
}
