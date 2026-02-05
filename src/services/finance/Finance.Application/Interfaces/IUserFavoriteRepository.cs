namespace Finance.Application.Interfaces;

public interface IUserFavoriteRepository
{
    Task<IReadOnlyList<Guid>> GetFavoriteCurrencyIdsAsync(
        Guid userId,
        CancellationToken ct);
}
