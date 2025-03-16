using Microsoft.EntityFrameworkCore.Design;

namespace BlazorSample.Infra.EntityFrameworkCore.DbContexts;


public class DbContextDesignTimeFactory : IDesignTimeDbContextFactory<BlazorSampleDbContext>
{
    public BlazorSampleDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BlazorSampleDbContext>();
        optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=BlazorSampleDb;Trusted_Connection=True;MultipleActiveResultSets=true");
        return new BlazorSampleDbContext(optionsBuilder.Options);
    }
}
