using BlazorSample.Constants;
using BlazorSample.Domain.Infra;


using Dotnetydd.Permission.Definition;
using Dotnetydd.Permission.PermissionGrant;
using Microsoft.Extensions.Hosting;

namespace BlazorSample.Infra.EntityFrameworkCore.Migrations
{

    public class EfCoreMigrationHostService : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHostEnvironment _env;



        public EfCoreMigrationHostService(
            IServiceScopeFactory scopeFactory,
            IHostEnvironment env
            )
        {
            _scopeFactory = scopeFactory;
            _env = env;

        }



        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (IServiceScope serviceScope = _scopeFactory.CreateAsyncScope())
            {
                foreach (var dbcontext in DataContextTypeCache.GetApplicationDataContextList())
                {
                    var db = serviceScope.ServiceProvider.GetService(dbcontext);
                    if (db is DbContext ctx)
                    {
                        await ctx.Database.MigrateAsync(cancellationToken);
                    }
                }
            }

            using IServiceScope seedServiceScope = _scopeFactory.CreateAsyncScope();
            await SeedSaPermissions(seedServiceScope.ServiceProvider);
            await SeedData(seedServiceScope.ServiceProvider);

        }



        async Task SeedData(IServiceProvider service)
        {
            var seeders = service.GetServices<IDataSeedProvider>();
            if (!seeders.Any())
            {
                return;
            }
            foreach (var seeder in seeders.OrderByDescending(x => x.Sorts).ToList())
            {
                await seeder.SeedAsync(service);
            }
        }

        async Task SeedSaPermissions(IServiceProvider service)
        {
            var permissionManager = service.GetService<IPermissionDefinitionManager>();
            var permissionStore = service.GetService<IPermissionGrantStore>();

            var permission = permissionManager.GetPermissions().Select(x => x.Name);
            var saPermission = await permissionStore.GetListAsync(DomainConstantValue.PERMISSION_PROVIDER_NAME_ROLE, DomainConstantValue.SA_ROLE);
            if (!saPermission.Any())
            {
                await permissionStore.SaveAsync(permission.ToArray(), DomainConstantValue.PERMISSION_PROVIDER_NAME_ROLE, DomainConstantValue.SA_ROLE);
            }
        }



        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}