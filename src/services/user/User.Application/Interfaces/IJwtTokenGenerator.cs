namespace User.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId, string name);
}
