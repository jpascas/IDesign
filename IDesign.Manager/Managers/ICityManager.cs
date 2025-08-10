using IDesign.Access.Entities;

namespace IDesign.Manager;

public interface ICityManager
{
    Task<List<City>> GetAllAsync();
    Task<City?> GetByIdAsync(int id);
    Task AddAsync(City city);
    Task UpdateAsync(City city);
    Task DeleteAsync(int id);
}
