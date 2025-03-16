using BlazorSample.Application.AppServices.Generics;
using BlazorSample.Application.Models.Product;
using Dotnetydd.Tools.Models;

namespace BlazorSample.Application.AppServices.Product;

public interface IProductAppService
    : ICrudAppService<string, ProductDto, ProductPagedRequest, ProductListItemDto, ProductUpdateRequest, ProductCreateRequest>
{
    Task<Return<ProductDto, string>> GetByNameAsync(string productName,CancellationToken cancellationToken=default);

    Task<Return<ProductDto, string>> GetByNameWithDapperAsync(string productName,CancellationToken cancellationToken=default);
}