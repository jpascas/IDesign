using Microsoft.EntityFrameworkCore;

namespace IDesign.Access;

public interface ICityRepository
{
    Task<List<City>> GetAllAsync();
    Task<City?> GetByIdAsync(int id);
    Task AddAsync(City city);
    Task UpdateAsync(City city);
    Task DeleteAsync(int id);
}
