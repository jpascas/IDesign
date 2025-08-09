using IDesign.Access;

namespace IDesign.Manager;

public class CityManager : ICityManager
{
    private readonly ICityRepository _repo;
    public CityManager(ICityRepository repo) => _repo = repo;
    public Task<List<City>> GetAllAsync() => _repo.GetAllAsync();
    public Task<City?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
    public Task AddAsync(City city) => _repo.AddAsync(city);
    public Task UpdateAsync(City city) => _repo.UpdateAsync(city);
    public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
}
