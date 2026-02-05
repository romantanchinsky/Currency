using User.Domain.Entities;

namespace User.Application.Interfaces;

public interface IUserRepository
{
    Task<UserAccount?> GetByNameAsync(string name, CancellationToken ct);
    Task AddAsync(UserAccount user, CancellationToken ct);
}
