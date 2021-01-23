using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscraper.Core.Storage
{
    public class OmniscraperDbContextFactory : IDesignTimeDbContextFactory<OmniscraperDbContext>
    {
        public OmniscraperDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OmniscraperDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Database=omniscraper;Username=postgres;Password=root123")
                .UseSnakeCaseNamingConvention();

            return new OmniscraperDbContext(optionsBuilder.Options);
        }
    }
}
