using BlazorSample.Domain.Aggregates.App;
using BlazorSample.Domain.Aggregates.EventLogs;
using BlazorSample.Domain.Aggregates.Product;
using BlazorSample.Domain.Aggregates.System;
using BlazorSample.Infra.EntityFrameworkCore.EntityTypeConfig;
using BlazorSample.Uow;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace BlazorSample.Infra.EntityFrameworkCore.DbContexts;

public class BlazorSampleDbContext : BaseDbContext<BlazorSampleDbContext>, IDataContext
{
    public BlazorSampleDbContext(DbContextOptions<BlazorSampleDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Device> Device { get; set; }

    public virtual DbSet<DeviceTag> DeviceTag { get; set; }

    public virtual DbSet<PermissionGrant> PermissionGrants { get; set; }

    public virtual DbSet<IntegrationEventLogEntry> IntegrationEventLogEntry { get; set; }


    public virtual DbSet<Application> Application { get; set; }

    public virtual DbSet<Role> Role { get; set; }

    public virtual DbSet<UIResource> UIResource { get; set; }

    public virtual DbSet<APIResource> APIResource { get; set; }

    public virtual DbSet<User> User { get; set; }

    public virtual DbSet<UIResourceRole> UIResourceRole { get; set; }

    public virtual DbSet<APIResourceRole> APIResourceRole { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // 不能去除，对租户，软删除过滤器
        modelBuilder.ApplyConfiguration(new DeviceEntityTypeConfiguration())
            .ApplyConfiguration(new DeviceTagEntityTypeConfiguration())
            .ApplyConfiguration(new ProductEntityTypeConfiguration())
            .ApplyConfiguration(new PermissionEntityTypeConfiguration());


        modelBuilder.ApplyConfiguration(new ApplicationConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new UIResourceConfiguration());
        modelBuilder.ApplyConfiguration(new APIResourceConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());

        modelBuilder.Entity<IntegrationEventLogEntry>().ToTable("IntegrationEventLog");
    }

    public class ApplicationConfiguration : IEntityTypeConfiguration<Application>
    {
        public void Configure(EntityTypeBuilder<Application> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Name).IsRequired().HasMaxLength(100);
            builder.HasMany(a => a.Roles).WithOne().HasForeignKey(r => r.ApplicationId);
            builder.HasMany(a => a.UIResources).WithOne(u => u.Application).HasForeignKey(u => u.ApplicationId);
            builder.HasMany(a => a.APIResources).WithOne(a => a.Application).HasForeignKey(a => a.ApplicationId);
        }
    }

    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Name).IsRequired().HasMaxLength(100);
            builder.HasMany(r => r.UIResourceRoles).WithOne(ur => ur.Role).HasForeignKey(ur => ur.RoleId).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(r => r.APIResourceRoles).WithOne(ar => ar.Role).HasForeignKey(ar => ar.RoleId).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(r => r.Users).WithMany(u => u.Roles);
        }
    }

    public class UIResourceConfiguration : IEntityTypeConfiguration<UIResource>
    {
        public void Configure(EntityTypeBuilder<UIResource> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Name).IsRequired().HasMaxLength(100);
            builder.HasOne(u => u.Application).WithMany(a => a.UIResources).HasForeignKey(u => u.ApplicationId);
            builder.HasMany(u => u.UIResourceRoles).WithOne(ur => ur.UIResource).HasForeignKey(ur => ur.UIResourceId).OnDelete(DeleteBehavior.NoAction);
        }
    }

    public class APIResourceConfiguration : IEntityTypeConfiguration<APIResource>
    {
        public void Configure(EntityTypeBuilder<APIResource> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Name).IsRequired().HasMaxLength(100);
            builder.HasOne(a => a.Application).WithMany(a => a.APIResources).HasForeignKey(a => a.ApplicationId);
            builder.HasMany(a => a.APIResourceRoles).WithOne(ar => ar.APIResource).HasForeignKey(ar => ar.APIResourceId).OnDelete(DeleteBehavior.NoAction);
        }
    }

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Name).IsRequired().HasMaxLength(100);
            builder.HasMany(u => u.Roles).WithMany(r => r.Users);
        }
    }

    public class UIResourceRoleConfiguration : IEntityTypeConfiguration<UIResourceRole>
    {
        public void Configure(EntityTypeBuilder<UIResourceRole> builder)
        {
            builder.HasKey(ur => new { ur.UIResourceId, ur.RoleId });
            builder.HasOne(ur => ur.UIResource).WithMany(u => u.UIResourceRoles).HasForeignKey(ur => ur.UIResourceId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(ur => ur.Role).WithMany(r => r.UIResourceRoles).HasForeignKey(ur => ur.RoleId).OnDelete(DeleteBehavior.NoAction);
        }
    }

    public class APIResourceRoleConfiguration : IEntityTypeConfiguration<APIResourceRole>
    {
        public void Configure(EntityTypeBuilder<APIResourceRole> builder)
        {
            builder.HasKey(ar => new { ar.APIResourceId, ar.RoleId });
            builder.HasOne(ar => ar.APIResource).WithMany(a => a.APIResourceRoles).HasForeignKey(ar => ar.APIResourceId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(ar => ar.Role).WithMany(r => r.APIResourceRoles).HasForeignKey(ar => ar.RoleId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
