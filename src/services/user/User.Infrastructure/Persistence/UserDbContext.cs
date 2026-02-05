using Microsoft.EntityFrameworkCore;
using User.Domain.Entities;

namespace User.Infrastructure.Persistence;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options) { }

    public DbSet<UserAccount> Users => Set<UserAccount>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserAccount>(b =>
        {
            b.ToTable("user_account");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.Name).IsUnique();
            b.Property(x => x.Name).IsRequired();
            b.Property(x => x.PasswordHash).IsRequired();
        });

        var passwordHash = BCrypt.Net.BCrypt.HashPassword("123");

        //seeding users
        modelBuilder.Entity<UserAccount>().HasData(
            new UserAccount
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "user1",
                PasswordHash = passwordHash
            },
            new UserAccount
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "user2",
                PasswordHash = passwordHash
            }
        );
        base.OnModelCreating(modelBuilder);
    }
}
