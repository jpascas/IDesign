using IDesign.Access.Entities;

namespace IDesign.Manager;

public interface ICountryManager
{
    Task<List<Country>> GetAllAsync();
    Task<Country?> GetByIdAsync(int id);
    Task AddAsync(Country country);
    Task UpdateAsync(Country country);
    Task DeleteAsync(int id);
}
