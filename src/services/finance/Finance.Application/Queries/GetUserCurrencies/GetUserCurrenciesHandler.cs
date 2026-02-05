using Finance.Application.DTOs;
using Finance.Application.Interfaces;
using MediatR;

namespace Finance.Application.Queries.GetUserCurrencies;

public class GetUserCurrenciesHandler
    : IRequestHandler<GetUserCurrenciesQuery, IReadOnlyList<CurrencyDto>>
{
    private readonly ICurrencyRepository _currencyRepo;
    private readonly IUserFavoriteRepository _favoriteRepo;

    public GetUserCurrenciesHandler(
        ICurrencyRepository currencyRepo,
        IUserFavoriteRepository favoriteRepo)
    {
        _currencyRepo = currencyRepo;
        _favoriteRepo = favoriteRepo;
    }

    public async Task<IReadOnlyList<CurrencyDto>> Handle(
        GetUserCurrenciesQuery request,
        CancellationToken ct)
    {
        var favoriteIds = await _favoriteRepo
            .GetFavoriteCurrencyIdsAsync(request.UserId, ct);

        var currencies = favoriteIds.Count == 0
            ? await _currencyRepo.GetAllAsync(ct)
            : await _currencyRepo.GetByIdsAsync(favoriteIds, ct);

        return currencies
            .Select(c => new CurrencyDto(c.Id, c.Name, c.Rate))
            .ToList();
    }
}
