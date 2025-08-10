using IDesign.Access.Entities;
using Microsoft.EntityFrameworkCore;

namespace IDesign.Access.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DesignDbContext _context;
    public UserRepository(DesignDbContext context) => _context = context;

    public async Task<User?> GetByIdAsync(long id) =>
        await _context.Set<User>().FindAsync(id);

    public async Task<User?> GetByEmailAsync(string email) =>
        await _context.Set<User>().FirstOrDefaultAsync(u => u.Email == email);

    public async Task<List<User>> GetAllAsync() =>
        await _context.Set<User>().ToListAsync();

    public async Task AddAsync(User user)
    {
        _context.Set<User>().Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Set<User>().Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var user = await _context.Set<User>().FindAsync(id);
        if (user != null)
        {
            _context.Set<User>().Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}