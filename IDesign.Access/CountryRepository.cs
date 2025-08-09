using Microsoft.EntityFrameworkCore;

namespace IDesign.Access;

public class CountryRepository : ICountryRepository
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
