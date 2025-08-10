using IDesign.Access.Entities;
using IDesign.Access.Repositories;

namespace IDesign.Manager;

public class CountryManager : ICountryManager
{
    private readonly ICountryRepository _repo;
    public CountryManager(ICountryRepository repo) => _repo = repo;
    public Task<List<Country>> GetAllAsync() => _repo.GetAllAsync();
    public Task<Country?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
    public Task AddAsync(Country country) => _repo.AddAsync(country);
    public Task UpdateAsync(Country country) => _repo.UpdateAsync(country);
    public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
}
