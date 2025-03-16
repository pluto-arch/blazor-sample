using BlazorSample.Application.Models.Product;
using BlazorSample.Domain.DomainEvents.Product;
using BlazorSample.Domain.Infra.Repository;
using BlazorSample.Infra.EntityFrameworkCore.DbContexts;
using BlazorSample.Infra.Utils;
using BlazorSample.Uow;
using ProductAgg = BlazorSample.Domain.Aggregates.Product;


namespace BlazorSample.Application.Command.Product
{
    [AutoResolveDependency]
    public partial class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
    {
        [AutoInject]
        private readonly IMapper _mapper;

        [AutoInject]
        private readonly IUnitOfWork<BlazorSampleDbContext> _uow;

        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var repository = _uow.Resolve<IEfRepository<BlazorSample.Domain.Aggregates.Product.Product>>();
            var entity = _mapper.Map<ProductAgg.Product>(request);
            entity.Id = SnowFlakeId.Generator.GetUniqueId();
            entity.CreationTime = DateTimeOffset.Now;
            entity.AddDomainEvent(new NewProductCreateDomainEvent(entity));
            entity = await repository.InsertAsync(entity, cancellationToken: cancellationToken);
            await _uow.CompleteAsync(cancellationToken);
            return _mapper.Map<ProductDto>(entity);
        }
    }
}
