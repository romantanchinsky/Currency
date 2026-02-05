using Finance.Application.DTOs;
using MediatR;

namespace Finance.Application.Queries.GetUserCurrencies;

public record GetUserCurrenciesQuery(Guid UserId)
    : IRequest<IReadOnlyList<CurrencyDto>>;
