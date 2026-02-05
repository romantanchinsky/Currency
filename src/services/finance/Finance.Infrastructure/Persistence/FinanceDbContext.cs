using Finance.Domain.Entities;
using Finance.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Finance.Infrastructure.Persistence;

public class FinanceDbContext : DbContext
{
    public FinanceDbContext(DbContextOptions<FinanceDbContext> options)
        : base(options) { }

    public DbSet<Currency> Currencies => Set<Currency>();
    public DbSet<UserFavorite> UserCurrencies => Set<UserFavorite>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Currency>(b =>
        {
            b.ToTable("currency");
            b.HasKey(x => x.Id);
            b.Property(x => x.Name).IsRequired();
            b.Property(x => x.Rate).HasPrecision(18, 4);
        });

        modelBuilder.Entity<UserFavorite>(b =>
        {
            b.ToTable("user_favorite");
            b.HasKey(x => new { x.UserId, x.CurrencyId });
        });

        modelBuilder.Entity<Currency>().HasData(
            new Currency()
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Name = "AAA",
                Rate = 90
            },
            new Currency()
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                Name = "BBB",
                Rate = 80
            }
        );

        modelBuilder.Entity<UserFavorite>().HasData(
            new UserFavorite
            {
                UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                CurrencyId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
            },
            new UserFavorite
            {
                UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                CurrencyId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb")
            }
        );

        base.OnModelCreating(modelBuilder);
    }
}



