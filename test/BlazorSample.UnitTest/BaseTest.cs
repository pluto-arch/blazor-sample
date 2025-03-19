using BlazorSample.Application;
using BlazorSample.Constants;
using BlazorSample.Domain;
using BlazorSample.Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Infrastructure;
using BlazorSample.Infra.EntityFrameworkCore.DbContexts;
using BlazorSample.Domain.Infra;
using BlazorSample.Infra.EntityFrameworkCore;
using BlazorSample.Infra.EntityFrameworkCore.ConnectionStringResolve;
using BlazorSample.Infra.EntityFrameworkCore.Interceptor;
using BlazorSample.Infra.Global;
using BlazorSample.Infra.Providers;

#pragma warning disable NUnit1032

namespace BlazorSample.UnitTest
{
    public class BaseTest
    {
        protected IServiceProvider ServiceProvider;

        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();


            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddLogging();

            #region »º´æ

            services.AddMemoryCache(options => { options.SizeLimit = 10240; });

            #endregion

            services.AddApplicationModule(configuration);


            services.AddSingleton<GlobalAccessor.CurrentUserAccessor>();
            services.AddScoped<GlobalAccessor.CurrentUser>();
            services.AddTransient<IDomainEventDispatcher, MediatrDomainEventDispatcher>();

            #region BlazorSample DbContext

            services.AddKeyedSingleton<IConnectionStringResolve, DefaultConnectionStringResolve>(nameof(BlazorSampleDbContext));

            services.AddEfCoreInfraComponent<BlazorSampleDbContext>((serviceProvider,optionsBuilder) =>
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=BlazorSampleDb;Trusted_Connection=True;MultipleActiveResultSets=true");

                var mediator = serviceProvider.GetService<IDomainEventDispatcher>() ?? NullDomainEventDispatcher.Instance;
                optionsBuilder.AddInterceptors(new DataChangeSaveChangesInterceptor(mediator));

            });
            services.AddEfUnitofWork<BlazorSampleDbContext>();

            #endregion



            services.AddDomainModule();



            ServiceProvider = services.BuildServiceProvider();

            using var sc = ServiceProvider.CreateScope();
            var ctx = sc.ServiceProvider.GetRequiredService<BlazorSampleDbContext>();
            ctx.Database.EnsureCreated();
        }
    }
}