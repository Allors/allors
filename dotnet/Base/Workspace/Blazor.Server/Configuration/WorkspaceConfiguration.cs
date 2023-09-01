// <copyright file="ServiceCollectionExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the DomainTest type.</summary>

namespace Allors.Workspace.Configuration
{
    using Allors.Workspace.Adapters;
    using Domain;
    using Meta.Static;
    using Services;
    using System.Globalization;
    using System.Security.Claims;

    public static class WorkspaceConfiguration
    {
        public static void AddAllorsWorkspace(this IServiceCollection services)
        {
            var metaPopulation = new MetaBuilder().Build();
            var objectFactory = new ReflectionObjectFactory(metaPopulation, typeof(Person));

            var servicesBuilder = () => new WorkspaceServices(objectFactory, metaPopulation);
            var configuration = new Adapters.Direct.Configuration("Default", metaPopulation);

            services.AddScoped<Connection>(serviceProvider =>
            {
                var database = serviceProvider.GetRequiredService<IDatabaseService>().Database;
                var claimsPrincipalService = serviceProvider.GetRequiredService<IClaimsPrincipalService>();
                var user = claimsPrincipalService.User;
                var claim = user.FindFirst(ClaimTypes.NameIdentifier);
                var userId = claim != null ? int.Parse(claim.Value, CultureInfo.InvariantCulture) : 0;
                return new Adapters.Direct.Connection(configuration, database, servicesBuilder) { UserId = userId };
            });

            services.AddScoped<IWorkspaceFactory>(serviceProvider =>
            {
                var databaseConnection = serviceProvider.GetRequiredService<Connection>();
                return new FuncWorkspaceFactory(() => databaseConnection.CreateWorkspace());
            });
        }
    }
}
