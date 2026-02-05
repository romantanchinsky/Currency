using Finance.Application.Interfaces;
using Finance.Application.Queries.GetUserCurrencies;
using Finance.Domain.Entities;
using FluentAssertions;
using Moq;

public class GetUserCurrenciesHandlerTests
{
    [Fact]
    public async Task ReturnsAllCurrencies_WhenUserHasNoPreferences()
    {
        var currencyRepo = new Mock<ICurrencyRepository>();
        currencyRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Currency>
            {
                new() { Id = Guid.NewGuid(), Name = "USD", Rate = 90 },
                new() { Id = Guid.NewGuid(), Name = "EUR", Rate = 100 }
            });

        var userCurrencyRepo = new Mock<IUserFavoriteRepository>();
        userCurrencyRepo.Setup(r => r.GetFavoriteCurrencyIdsAsync(
            It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Guid>());

        var handler = new GetUserCurrenciesHandler(
            currencyRepo.Object,
            userCurrencyRepo.Object);

        var result = await handler.Handle(
            new GetUserCurrenciesQuery(Guid.NewGuid()),
            CancellationToken.None);

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task ReturnsOnlyFavoriteCurrencies_WhenUserHasPreferences()
    {
        var usdId = Guid.NewGuid();

        var currencyRepo = new Mock<ICurrencyRepository>();
        currencyRepo.Setup(r => r.GetByIdsAsync(
                It.IsAny<IEnumerable<Guid>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Currency>
            {
            new() { Id = usdId, Name = "USD", Rate = 90 }
            });

        var userCurrencyRepo = new Mock<IUserFavoriteRepository>();
        userCurrencyRepo.Setup(r => r.GetFavoriteCurrencyIdsAsync(
                It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Guid> { usdId });

        var handler = new GetUserCurrenciesHandler(
            currencyRepo.Object,
            userCurrencyRepo.Object);

        var result = await handler.Handle(
            new GetUserCurrenciesQuery(Guid.NewGuid()),
            CancellationToken.None);

        result.Should().ContainSingle(c => c.Name == "USD");
    }
    [Fact]
    public async Task ReturnsEmpty_WhenNoCurrenciesExist()
    {
        var currencyRepo = new Mock<ICurrencyRepository>();
        currencyRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Currency>());

        var userCurrencyRepo = new Mock<IUserFavoriteRepository>();
        userCurrencyRepo.Setup(r => r.GetFavoriteCurrencyIdsAsync(
            It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Guid>());

        var handler = new GetUserCurrenciesHandler(
            currencyRepo.Object,
            userCurrencyRepo.Object);

        var result = await handler.Handle(
            new GetUserCurrenciesQuery(Guid.NewGuid()),
            CancellationToken.None);

        result.Should().BeEmpty();
    }

}
