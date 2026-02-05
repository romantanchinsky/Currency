using Finance.Domain.Entities;

namespace Finance.Application.Interfaces;

public interface ICurrencyRepository
{
    Task<IReadOnlyList<Currency>> GetByIdsAsync(
        IEnumerable<Guid> ids,
        CancellationToken ct);

    Task<IReadOnlyList<Currency>> GetAllAsync(CancellationToken ct);
}
