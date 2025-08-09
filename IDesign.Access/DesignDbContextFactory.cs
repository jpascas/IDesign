using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace IDesign.Access;

public class DesignDbContextFactory : IDesignTimeDbContextFactory<DesignDbContext>
{
    public DesignDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DesignDbContext>();
        optionsBuilder.UseSqlite("Data Source=design.db");
        return new DesignDbContext(optionsBuilder.Options);
    }
}
