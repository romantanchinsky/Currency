using Microsoft.EntityFrameworkCore;
using User.Application.Interfaces;
using User.Domain.Entities;
using User.Infrastructure.Persistence;

namespace User.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _db;

    public UserRepository(UserDbContext db)
    {
        _db = db;
    }

    public Task<UserAccount?> GetByNameAsync(string name, CancellationToken ct)
        => _db.Users.FirstOrDefaultAsync(x => x.Name == name, ct);

    public async Task AddAsync(UserAccount user, CancellationToken ct)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);
    }
}
