using System.Data;

namespace BlazorSample.Application.AppServices.Queries.ConnectionFactory
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
