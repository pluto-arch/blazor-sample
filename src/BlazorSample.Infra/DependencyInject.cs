using BlazorSample.Constants;
using BlazorSample.Domain.Infra;
using BlazorSample.Infra.EntityFrameworkCore;
using BlazorSample.Infra.EntityFrameworkCore.ConnectionStringResolve;
using BlazorSample.Infra.EntityFrameworkCore.DbContexts;
using BlazorSample.Infra.EntityFrameworkCore.Interceptor;
using BlazorSample.Infra.Global;
using BlazorSample.Infra.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorSample.Infra
{
    public static class DependencyInject
    {
        public static IServiceCollection AddInfraModule(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddSingleton<GlobalAccessor.CurrentUserAccessor>();
            service.AddScoped<GlobalAccessor.CurrentUser>();
            service.AddTransient<IDomainEventDispatcher, MediatrDomainEventDispatcher>();




            #region BlazorSample DbContext

            service.AddKeyedSingleton<IConnectionStringResolve, DefaultConnectionStringResolve>(nameof(BlazorSampleDbContext));
            var migrationAssembly = Assembly.GetCallingAssembly().GetName().Name;
            service.AddEfCoreInfraComponent<BlazorSampleDbContext>((serviceProvider,optionsBuilder) =>
            {
                optionsBuilder.UseSqlServer(configuration.GetConnectionString(InfraConstantValue.DEFAULT_CONNECTIONSTRING_NAME),
                    sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(migrationAssembly);
                    });

                var mediator = serviceProvider.GetService<IDomainEventDispatcher>() ?? NullDomainEventDispatcher.Instance;
                optionsBuilder.AddInterceptors(new DataChangeSaveChangesInterceptor(mediator));


            });
            service.AddEfUnitofWork<BlazorSampleDbContext>();

            #endregion



            return service;
        }
    }
}