using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using User.Infrastructure.Persistence;
using Finance.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        services.AddDbContext<UserDbContext>(o =>
            o.UseNpgsql(ctx.Configuration.GetConnectionString("Default")));

        services.AddDbContext<FinanceDbContext>(o =>
            o.UseNpgsql(ctx.Configuration.GetConnectionString("Default")));
    })
    .Build();

using var scope = host.Services.CreateScope();

Console.WriteLine("Applying UserDbContext migrations...");
scope.ServiceProvider.GetRequiredService<UserDbContext>()
    .Database.Migrate();

Console.WriteLine("Applying FinanceDbContext migrations...");
scope.ServiceProvider.GetRequiredService<FinanceDbContext>()
    .Database.Migrate();

Console.WriteLine("All migrations applied");
