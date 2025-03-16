namespace BlazorSample.Domain
{
    public static class DependencyInject
    {
        public static IServiceCollection AddDomainModule(this IServiceCollection service)
        {
            service.AutoInjectBlazorSample_Domain();
            return service;
        }
    }
}