using IDesign.Access.Entities;
using IDesign.Access.Repositories;

namespace IDesign.Manager.Managers;

public class UserManager : IUserManager
{
    private readonly IUserRepository _repo;
    public UserManager(IUserRepository repo) => _repo = repo;

    public Task<User?> GetByIdAsync(long id) => _repo.GetByIdAsync(id);
    public Task<User?> GetByEmailAsync(string email) => _repo.GetByEmailAsync(email);
    public Task<List<User>> GetAllAsync() => _repo.GetAllAsync();
    public Task AddAsync(User user) => _repo.AddAsync(user);
    public Task UpdateAsync(User user) => _repo.UpdateAsync(user);
    public Task DeleteAsync(long id) => _repo.DeleteAsync(id);
}