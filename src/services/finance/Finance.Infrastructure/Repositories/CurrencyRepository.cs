using Finance.Application.Interfaces;
using Finance.Domain.Entities;
using Finance.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Finance.Infrastructure.Repositories;

public class CurrencyRepository : ICurrencyRepository
{
    private readonly FinanceDbContext _db;

    public CurrencyRepository(FinanceDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<Currency>> GetAllAsync(CancellationToken ct)
        => await _db.Currencies
        .AsNoTracking()
        .ToListAsync(ct);

    public async Task<IReadOnlyList<Currency>> GetByIdsAsync(
        IEnumerable<Guid> ids,
        CancellationToken ct)
        => await _db.Currencies
            .AsNoTracking()
            .Where(c => ids.Contains(c.Id))
            .ToListAsync(ct);
}
