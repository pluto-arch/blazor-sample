using BlazorSample.Application.AppServices.Product;
using BlazorSample.Application.Models.Product;
using BlazorSample.Domain.Aggregates.Product;
using BlazorSample.Domain.Infra.Repository;
using BlazorSample.Infra.EntityFrameworkCore.DbContexts;
using BlazorSample.Uow;

namespace BlazorSample.UnitTest
{
    public class DemoTest : BaseTest
    {

        [Test]
        public async Task Test()
        {
                var productService = ServiceProvider.GetService<IProductAppService>();

                await productService.CreateAsync(new ProductCreateRequest
                {
                    Id = "xxx",
                    Name = "Hello",
                    Remark = "ddddd"
                });

                var res = await productService.GetListAsync(new ProductPagedRequest
                {
                    PageNo = 1,
                    PageSize = 20,
                });

                Assert.That(res != null, Is.True);
        }


        [Test]
        public async Task UnitOfWorkTest()
        {
            var uow = ServiceProvider.GetService<IUnitOfWork<BlazorSampleDbContext>>();

            var code = uow.GetHashCode();

            var repa = uow.Resolve<IEfRepository<Product>>();


            await using var subUow = uow.BeginNew();
            var code2=subUow.ServiceProvider.GetHashCode();
            Assert.That(code!=code2,Is.True);
            var rep2= subUow.Resolve<IEfRepository<Product>>();
            Assert.That(repa != rep2, Is.True);
            Assert.That(repa.DataContext.GetHashCode()!=rep2.DataContext.GetHashCode(),Is.True);

        }

    }
}
