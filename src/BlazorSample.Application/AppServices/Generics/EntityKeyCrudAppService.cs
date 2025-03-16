using BlazorSample.Application.Models.Generics;
using BlazorSample.Domain.Infra;
using BlazorSample.Infra.EntityFrameworkCore.DbContexts;
using BlazorSample.Uow;

namespace BlazorSample.Application.AppServices.Generics
{
    public class EntityKeyCrudAppService<TEntity, TKey, TDto, TGetListRequest, TListItemDto, TUpdateRequest, TCreateRequest>
    : AlternateKeyCrudAppService<TEntity, TKey, TDto, TGetListRequest, TListItemDto, TUpdateRequest, TCreateRequest>
    where TEntity : BaseEntity<TKey>
    where TGetListRequest : PageRequest
    {

        /// <inheritdoc />
        public EntityKeyCrudAppService(IUnitOfWork<BlazorSampleDbContext> uow, IMapper mapper) : base(uow, mapper)
        {
        }

        protected override async Task<TEntity> GetEntityByIdAsync(TKey id, CancellationToken cancellationToken = default) => await _repository.GetAsync(id, cancellationToken);

        protected override async Task DeleteByIdAsync(TKey id, CancellationToken cancellationToken = default) => await _repository.DeleteAsync(id, true, cancellationToken);
    }
}
