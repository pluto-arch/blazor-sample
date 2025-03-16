using BlazorSample.Application.Models.Product;

namespace BlazorSample.Application.Queries.Product
{
    public interface IProductQueries
    {
        Task<ProductDto> GetAsync(string id);
    }
}
