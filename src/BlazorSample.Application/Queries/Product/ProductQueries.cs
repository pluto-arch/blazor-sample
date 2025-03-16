using BlazorSample.Constants;
using BlazorSample.Application.AppServices.Queries.ConnectionFactory;
using BlazorSample.Application.Models.Product;
using System.Data;


namespace BlazorSample.Application.Queries.Product
{

    [Injectable(InjectLifeTime.Transient)]
    public class ProductQueries : IProductQueries
    {
        private readonly IDbConnectionFactory _factory;
        private readonly IConfiguration _configuration;


        public ProductQueries(IDbConnectionFactory factory, IConfiguration configuration
            )
        {
            _factory = factory;
            _configuration = configuration;
        }

        public async Task<ProductDto> GetAsync(string id)
        {
            using IDbConnection connection = _factory.CreateConnection();
            connection.ConnectionString = _configuration.GetConnectionString(InfraConstantValue.DEFAULT_CONNECTIONSTRING_NAME);
            connection.Open();
            await Task.Delay(1000);
            // TODO use dapper 等轻量查询工具
            return null;

        }
    }
}
