namespace Workspace.Blazor.Server;

using Services;
using ViewModels.Services;

public static class Program
{
    public static IServiceCollection AddViewModelsServices(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        // Services
        services.AddScoped<IMessageService, MessageService>();

        // ViewModels
        services.AddScoped<ViewModels.Features.Person.Edit.PageViewModel>();
        services.AddScoped<ViewModels.Features.Person.List.PageViewModel>();

        return services;
    }
}
