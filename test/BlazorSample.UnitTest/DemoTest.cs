using BlazorSample.Application.AppServices.Product;
using BlazorSample.Application.Models.Product;
using BlazorSample.Domain.Aggregates.App;
using BlazorSample.Domain.Aggregates.Product;
using BlazorSample.Domain.Infra.Repository;
using BlazorSample.Infra.EntityFrameworkCore.DbContexts;
using BlazorSample.Uow;
using Bogus.DataSets;
using static BlazorSample.Application.Permission.RolePermission;

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
            var uow = ServiceProvider.GetService<BlazorSampleDbContext>();

            var role = new List<Role>()
            {
                new Role
                {
                    Name="a"
                }
            };
            var app = new Domain.Aggregates.App.Application
            {
                Name = "Hello",
                Roles = role,
                APIResources = new List<APIResource>
                {
                    new APIResource
                    {
                        Name="ar1"
                    }
                },
                UIResources = new List<UIResource>
                {
                    new UIResource
                    {
                        Name="ar2"
                    }
                }
            };


            await uow.Application.AddRangeAsync(app);
            await uow.SaveChangesAsync();
        }

    }
}
