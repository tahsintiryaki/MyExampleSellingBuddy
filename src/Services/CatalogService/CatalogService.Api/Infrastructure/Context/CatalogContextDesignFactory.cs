using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Api.Infrastructure.Context
{
    public class CatalogContextDesignFactory : IDesignTimeDbContextFactory<CatalogContext>
    {
        public CatalogContext CreateDbContext(string[] args)
        {
            //"Data Source=c.;Initial Catalog=catalog;Persist Security Info=True;User ID=sa;Password=Salih123!"
            var optionsBuilder = new DbContextOptionsBuilder<CatalogContext>()
                .UseSqlServer("Data Source=.;Initial Catalog=catalog;Persist Security Info=True;User ID=sa;Password=sa123!");

            return new CatalogContext(optionsBuilder.Options);
        }
    }
}
