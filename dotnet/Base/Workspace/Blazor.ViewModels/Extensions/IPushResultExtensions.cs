namespace Workspace.Blazor.ViewModels;

using System.Text;
using Allors.Workspace;
using global::Workspace.Blazor.ViewModels.Services;

public static class IPushResultExtensions
{
    public static void HandleErrors(this IPushResult @this, IMessageService messageService)
    {
        if (@this.HasErrors)
        {
            if (@this.AccessErrors?.Count() > 0)
            {
                messageService.Show(@"You do not have the required rights.", @"Access Error");
            }
            else if (@this.DerivationErrors?.Count() > 0)
            {
                var message = new StringBuilder();
                foreach (var derivationError in @this.DerivationErrors)
                {
                    message.Append($" - {derivationError.Message}\n");
                }

                messageService.Show(message.ToString(), @"Derivation Errors");
            }
            else if (@this.VersionErrors?.Count() > 0 || @this.MissingErrors?.Count() > 0)
            {
                messageService.Show(@"Modifications were detected since last access.", @"Concurrency Error");
            }
            else
            {
                messageService.Show($@"{@this.ErrorMessage}", @"General Error");
            }
        }
    }
}
