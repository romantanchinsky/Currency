using Finance.Domain.Entities;
using Finance.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Xml.Serialization;

public class Worker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IHttpClientFactory _httpClientFactory;

    public Worker(
        IServiceScopeFactory scopeFactory,
        IHttpClientFactory httpClientFactory)
    {
        _scopeFactory = scopeFactory;
        _httpClientFactory = httpClientFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await UpdateRates(stoppingToken);
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }

    private async Task UpdateRates(CancellationToken ct)
    {
        var client = _httpClientFactory.CreateClient("cbr");
        var xml = await client.GetStringAsync(client.BaseAddress, ct);

        var serializer = new XmlSerializer(typeof(ValCurs));
        using var reader = new StringReader(xml);
        var data = (ValCurs)serializer.Deserialize(reader)!;

        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<FinanceDbContext>();

        foreach (var valute in data.Valutes)
        {
            var rate = decimal.Parse(
                valute.Value.Replace(',', '.'),
                CultureInfo.InvariantCulture);

            var currency = await db.Currencies
                .FirstOrDefaultAsync(x => x.Name == valute.CharCode, ct);

            if (currency == null)
            {
                db.Currencies.Add(new Currency
                {
                    Id = Guid.NewGuid(),
                    Name = valute.CharCode,
                    Rate = rate
                });
            }
            else
            {
                currency.Rate = rate;
            }
        }

        await db.SaveChangesAsync(ct);
    }
}
