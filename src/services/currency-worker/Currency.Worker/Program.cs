using System.Text;
using Finance.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = Host.CreateApplicationBuilder(args);

var cbrHttpClientOptions = new CbrHttpClientOptions();
builder.Configuration.Bind(CbrHttpClientOptions.SectionName, cbrHttpClientOptions);

builder.Services.AddHttpClient("cbr", httpClient =>
{
    httpClient.BaseAddress = new Uri(cbrHttpClientOptions.Uri);
});

builder.Services.AddDbContext<FinanceDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
