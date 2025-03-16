
using BlazorSample.Constants;


namespace BlazorSample.Infra.EntityFrameworkCore.ConnectionStringResolve;

public class DefaultConnectionStringResolve : IConnectionStringResolve
{
    protected readonly IConfiguration _configuration;
    public DefaultConnectionStringResolve(
        IConfiguration configuration
        )
    {
        _configuration = configuration;

    }

    public virtual Task<string> GetAsync(string connectionStringName = null)
    {
        connectionStringName ??= InfraConstantValue.DEFAULT_CONNECTIONSTRING_NAME;
        var defaultConnectionString = _configuration.GetConnectionString(connectionStringName);
        return Task.FromResult(defaultConnectionString);
    }
}