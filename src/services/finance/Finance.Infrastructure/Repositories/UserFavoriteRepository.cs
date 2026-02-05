using Finance.Application.Interfaces;
using Finance.Infrastructure.Models;
using Finance.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Finance.Infrastructure.Repositories;

public class UserFavoriteRepository : IUserFavoriteRepository
{
    private readonly FinanceDbContext _db;

    public UserFavoriteRepository(FinanceDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<Guid>> GetFavoriteCurrencyIdsAsync(
        Guid userId,
        CancellationToken ct)
    {
        return await _db.Set<UserFavorite>()
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(x => x.CurrencyId)
            .ToListAsync(ct);
    }
}
