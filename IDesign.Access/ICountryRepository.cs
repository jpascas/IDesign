using Microsoft.EntityFrameworkCore;

namespace IDesign.Access;

public interface ICountryRepository
{
    Task<List<Country>> GetAllAsync();
    Task<Country?> GetByIdAsync(int id);
    Task AddAsync(Country country);
    Task UpdateAsync(Country country);
    Task DeleteAsync(int id);
}
