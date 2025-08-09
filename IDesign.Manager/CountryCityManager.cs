using IDesign.Access;

namespace IDesign.Manager;

public interface ICountryManager
{
    Task<List<Country>> GetAllAsync();
    Task<Country?> GetByIdAsync(int id);
    Task AddAsync(Country country);
    Task UpdateAsync(Country country);
    Task DeleteAsync(int id);
}

public interface ICityManager
{
    Task<List<City>> GetAllAsync();
    Task<City?> GetByIdAsync(int id);
    Task AddAsync(City city);
    Task UpdateAsync(City city);
    Task DeleteAsync(int id);
}

public class CountryManager : ICountryManager
{
    private readonly CountryRepository _repo;
    public CountryManager(CountryRepository repo) => _repo = repo;
    public Task<List<Country>> GetAllAsync() => _repo.GetAllAsync();
    public Task<Country?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
    public Task AddAsync(Country country) => _repo.AddAsync(country);
    public Task UpdateAsync(Country country) => _repo.UpdateAsync(country);
    public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
}

public class CityManager : ICityManager
{
    private readonly CityRepository _repo;
    public CityManager(CityRepository repo) => _repo = repo;
    public Task<List<City>> GetAllAsync() => _repo.GetAllAsync();
    public Task<City?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
    public Task AddAsync(City city) => _repo.AddAsync(city);
    public Task UpdateAsync(City city) => _repo.UpdateAsync(city);
    public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
}
