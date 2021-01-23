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
            optionsBuilder.UseSqlite("Data Source=omniscraper.db");

            return new OmniscraperDbContext(optionsBuilder.Options);
        }
    }
}
