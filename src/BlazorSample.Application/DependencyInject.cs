using BlazorSample.Application.Behaviors;
using BlazorSample.Application.Permission;
using BlazorSample.Infra.EntityFrameworkCore.Repository;
using Dotnetydd.Permission.Checker;
using Dotnetydd.Permission.Definition;
using Dotnetydd.Permission.PermissionGrant;
using Dotnetydd.Permission.PermissionManager;
using Dotnetydd.Permission.ValueProvider;
using FastExpressionCompiler;
using Mapster;

namespace BlazorSample.Application
{
    public static class DependencyInject
    {
        public static IServiceCollection AddApplicationModule(this IServiceCollection services, IConfiguration configuration, IEnumerable<Assembly> assemblies = null)
        {
            assemblies ??= AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => !string.IsNullOrEmpty(x.FullName) && x.FullName.Contains("BlazorSample", StringComparison.OrdinalIgnoreCase));
            services.AddMapster(assemblies.ToArray());

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(TransactionBehavior<,>).Assembly));

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));


            #region permission
            services.AddScoped<IPermissionChecker, PermissionChecker>();
            // permission definition 
            services.AddSingleton<IPermissionDefinitionManager, DefaultPermissionDefinitionManager>();
            services.AddSingleton<IPermissionDefinitionProvider, PermissionDefinitionProvider>();

            services.AddScoped<IPermissionGrantStore, EfCorePermissionGrantStore>();
            services.AddScoped<IPermissionManager, CachedPermissionManager>();
            services.AddScoped<IPermissionValueProvider, RolePermissionValueProvider>();
            services.AddScoped<IPermissionValueProvider, UserPermissionValueProvider>();
            #endregion


            services.AutoInjectBlazorSample_Application();

            return services;
        }


        public static void AddMapster(this IServiceCollection services, params Assembly[] assemblies)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            TypeAdapterConfig.GlobalSettings.Compiler = exp => exp.CompileFast();
            config.Scan(assemblies);
            services.AddSingleton(config);
            services.AddSingleton<IMapper, ServiceMapper>();
        }
    }
}