using Microsoft.EntityFrameworkCore;

namespace IDesign.Access;

public class CityRepository : ICityRepository
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
