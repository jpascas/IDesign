using Microsoft.EntityFrameworkCore;

namespace IDesign.Access;

public class CountryRepository
{
    private readonly DesignDbContext _context;
    public CountryRepository(DesignDbContext context)
    {
        _context = context;
    }

    public async Task<List<Country>> GetAllAsync() => await _context.Countries.Include(c => c.Cities).ToListAsync();
    public async Task<Country?> GetByIdAsync(int id) => await _context.Countries.Include(c => c.Cities).FirstOrDefaultAsync(c => c.Id == id);
    public async Task AddAsync(Country country)
    {
        _context.Countries.Add(country);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Country country)
    {
        _context.Countries.Update(country);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(int id)
    {
        var country = await _context.Countries.FindAsync(id);
        if (country != null)
        {
            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();
        }
    }
}

public class CityRepository
{
    private readonly DesignDbContext _context;
    public CityRepository(DesignDbContext context)
    {
        _context = context;
    }

    public async Task<List<City>> GetAllAsync() => await _context.Cities.Include(c => c.Country).ToListAsync();
    public async Task<City?> GetByIdAsync(int id) => await _context.Cities.Include(c => c.Country).FirstOrDefaultAsync(c => c.Id == id);
    public async Task AddAsync(City city)
    {
        _context.Cities.Add(city);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(City city)
    {
        _context.Cities.Update(city);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(int id)
    {
        var city = await _context.Cities.FindAsync(id);
        if (city != null)
        {
            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
        }
    }
}
