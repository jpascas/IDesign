using IDesign.Access.Entities;
using IDesign.Access.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace IDesign.Access;

public class DesignDbContext : DbContext
{
    private readonly IPasswordHasher? _hasher;
    public DesignDbContext(DbContextOptions<DesignDbContext> options, IPasswordHasher? hasher = null) : base(options) {
        _hasher = hasher;
    }

    public DbSet<Country> Countries => Set<Country>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<User> Users => Set<User>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSeeding(async (context, seed) =>
        {
            DesignDbContext? designDbContext = context as DesignDbContext;
            if (designDbContext == null)
            {
                throw new InvalidOperationException("DesignDbContext is not configured correctly.");
            }

            if (!await designDbContext.Users.AnyAsync())
            {
                var users = GetUsersToSeed();

                await designDbContext.Users.AddRangeAsync(users);
                await designDbContext.SaveChangesAsync();
            }

            if (!await designDbContext.Countries.AnyAsync())
            {
                var countries = GetCountriesToSeed();

                await designDbContext.Countries.AddRangeAsync(countries);
                await designDbContext.SaveChangesAsync();

            }
        });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>()
            .HasMany(c => c.Cities)
            .WithOne(c => c.Country)
            .HasForeignKey(c => c.CountryId)
            .OnDelete(DeleteBehavior.Cascade);        
    }

    private List<Country> GetCountriesToSeed()
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

    private List<User> GetUsersToSeed()
    {
        if (_hasher == null)
        {
            throw new InvalidOperationException("Password hasher is not configured.");
        }

        return new List<User>
        {
            new User { Id = 1, Email = "admin@idesign.test", PasswordHash = _hasher.Hash("admin123"), Role = "Admin" },
            new User { Id = 2, Email = "user1@idesign.test", PasswordHash = _hasher.Hash("user123"), Role = "User" },
            new User { Id = 3, Email = "user2@idesign.test", PasswordHash = _hasher.Hash("user123"), Role = "User" }
        };
    }
}
