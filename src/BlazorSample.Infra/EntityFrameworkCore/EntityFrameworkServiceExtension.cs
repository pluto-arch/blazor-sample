﻿using BlazorSample.Constants;
using BlazorSample.Domain.Aggregates.Product;
using BlazorSample.Domain.Infra;
using BlazorSample.Domain.Infra.Repository;
using BlazorSample.Infra.EntityFrameworkCore.ConnectionStringResolve;
using BlazorSample.Infra.EntityFrameworkCore.DbContexts;
using BlazorSample.Infra.EntityFrameworkCore.Interceptor;
using BlazorSample.Infra.EntityFrameworkCore.Repository;
using BlazorSample.Uow;
using BlazorSample.Uow.EntityFrameworkCore;
using Dotnetydd.Specifications.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BlazorSample.Infra.EntityFrameworkCore;

public static class EntityFrameworkServiceExtension
{

    /// <summary>
    /// 添加efcore 组件
    /// </summary>
    /// <remarks>包括efcore dbcontext，默认仓储</remarks>
    /// <returns></returns>
    public static IServiceCollection AddEfCoreInfraComponent<TDbContext>(this IServiceCollection service, 
        Action<IServiceProvider,DbContextOptionsBuilder> optionsAction)
        where TDbContext:DbContext
    {
        service.AddDbContextFactory<TDbContext>(optionsAction, ServiceLifetime.Scoped);
        service.AddDefaultRepository<TDbContext>();
        return service;
    }

    /// <summary>
    /// 添加默认仓储
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    internal static void AddDefaultRepository<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext
    {
        var item = typeof(TDbContext);

        var entitTypies =
            from property in item.GetTypeInfo().GetProperties(BindingFlags.Instance | BindingFlags.Public)
            where IsAssignableToGenericType(property.PropertyType, typeof(DbSet<>)) &&
                  typeof(IEntity).IsAssignableFrom(property.PropertyType.GenericTypeArguments[0])
            select property.PropertyType.GenericTypeArguments[0];


        foreach (var entityType in entitTypies)
        {
            // efcore 仓储
            var defType = typeof(IEfRepository<>).MakeGenericType(entityType);
            var defType2 = typeof(IEfContextRepository<,>).MakeGenericType(item, entityType);
            var implementingType = EfRepositoryHelper.GetRepositoryType(item, entityType);
            services.RegisterScopedType(defType, implementingType);
            services.RegisterScopedType(defType2, implementingType);

            // 通用仓储
            var genericRepInterface = typeof(IGenericRepository<>).MakeGenericType(entityType);
            var genericRepInterfaceImpl = typeof(EfGenericRepository<,>).MakeGenericType(item,entityType);
            services.RegisterScopedType(genericRepInterface, genericRepInterfaceImpl);


            Type keyType = EntityHelper.FindPrimaryKeyType(entityType);
            if (keyType != null)
            {
                // efcore 指定context 的仓储
                var impl = EfRepositoryHelper.GetRepositoryType(item, entityType, keyType);
                services.RegisterScopedType(typeof(IEfRepository<,>).MakeGenericType(entityType, keyType), impl);
                services.RegisterScopedType(typeof(IEfContextRepository<,,>).MakeGenericType(item, entityType, keyType),
                    impl);
            }
        }
    }



    public static void AddEfUnitofWork<TDbContext>(this IServiceCollection services)
        where TDbContext:DbContext
    {
        var dbcontextType = typeof(TDbContext);
        services.RegisterScopedType(typeof(IUnitOfWork<>).MakeGenericType(dbcontextType), typeof(EfUnitOfWork<>).MakeGenericType(dbcontextType));
    }


    #region private


    private static IServiceCollection RegisterType(this IServiceCollection services, Type type, Type implementationType)
    {
        if (type.IsAssignableFrom(implementationType))
        {
            services.TryAddTransient(type, implementationType);
        }
        return services;
    }


    private static IServiceCollection RegisterScopedType(this IServiceCollection services, Type type, Type implementationType)
    {
        if (type.IsAssignableFrom(implementationType))
        {
            services.TryAddScoped(type, implementationType);
        }
        return services;
    }

    public static bool IsAssignableToGenericType(Type givenType, Type genericType)
    {
        TypeInfo typeInfo = givenType.GetTypeInfo();
        if (typeInfo.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
        {
            return true;
        }

        Type[] interfaces = typeInfo.GetInterfaces();
        foreach (Type type in interfaces)
        {
            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }
        }

        if (typeInfo.BaseType == null)
        {
            return false;
        }

        return IsAssignableToGenericType(typeInfo.BaseType, genericType);
    }
    #endregion
}