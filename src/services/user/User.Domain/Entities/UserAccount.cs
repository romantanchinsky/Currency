namespace User.Domain.Entities;

public class UserAccount
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
}