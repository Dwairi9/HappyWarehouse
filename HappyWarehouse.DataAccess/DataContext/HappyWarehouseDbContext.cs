using HappyWarehouse.DataAccess.Entities;
using HappyWarehouse.DataAccess.Interceptors;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HappyWarehouse.DataAccess.DataContext
{
    public class HappyWarehouseDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

        public HappyWarehouseDbContext(DbContextOptions<HappyWarehouseDbContext> options,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) : base(options)
        {
            _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableDetailedErrors();
            optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
            
            base.OnConfiguring(optionsBuilder);
        }

        #region DbSet
        public DbSet<Warehouse> Warehouses => Set<Warehouse>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<Country> Countries => Set<Country>();
        #endregion
    }
}
