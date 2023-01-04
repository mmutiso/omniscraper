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
            var serverVersion = new MySqlServerVersion(new Version(5,7));
            optionsBuilder.UseMySql("<connection string here>;", serverVersion) 
            .UseSnakeCaseNamingConvention();

            return new OmniscraperDbContext(optionsBuilder.Options);
        }
    }
}
