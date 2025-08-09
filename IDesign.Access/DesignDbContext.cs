using IDesign.Access.Entities;
using Microsoft.EntityFrameworkCore;

namespace IDesign.Access;

public class DesignDbContext : DbContext
{
    public DesignDbContext(DbContextOptions<DesignDbContext> options) : base(options) { }

    public DbSet<Country> Countries => Set<Country>();
    public DbSet<City> Cities => Set<City>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>()
            .HasMany(c => c.Cities)
            .WithOne(c => c.Country)
            .HasForeignKey(c => c.CountryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Country>()
               .HasData(GetCountriesToSeed());
    }

    private static IEnumerable<Country> GetCountriesToSeed()
    {
        var countries = new List<Country>
        {
            new Country { Id = 1, Name = "USA" },
            new Country { Id = 2, Name = "Canada" },
            new Country { Id = 3, Name = "Mexico" },
            new Country { Id = 4, Name = "Brazil" },
            new Country { Id = 5, Name = "Argentina" },
            new Country { Id = 6, Name = "Colombia" },
            new Country { Id = 7, Name = "Chile" },
            new Country { Id = 8, Name = "Peru" },
            new Country { Id = 9, Name = "Venezuela" },
            new Country { Id = 10, Name = "Ecuador" }
        };

        var cities = new List<City>();
        int cityId = 1;
        foreach (var country in countries)
        {
            for (int i = 1; i <= 10; i++)
            {
                cities.Add(new City
                {
                    Id = cityId++,
                    Name = $"{country.Name} City {i}",
                    CountryId = country.Id
                });
            }
        }
        // Assign cities to countries
        foreach (var country in countries)
        {
            country.Cities = cities.Where(c => c.CountryId == country.Id).ToList();
        }
        return countries;
    }
}
