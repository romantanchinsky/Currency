using FluentAssertions;
using Moq;
using User.Application.Commands.LoginUser;
using User.Application.Interfaces;
using User.Domain.Entities;

public class LoginUserHandlerTests
{
    [Fact]
    public async Task Login_WhenCredentialsValid_ReturnsToken()
    {
        var password = "123";
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        var user = new UserAccount
        {
            Id = Guid.NewGuid(),
            Name = "test",
            PasswordHash = hashedPassword
        };

        var repo = new Mock<IUserRepository>();
        repo.Setup(r => r.GetByNameAsync("test", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var jwt = new Mock<IJwtTokenGenerator>();
        jwt.Setup(j => j.GenerateToken(user.Id, "test"))
            .Returns("token");

        var handler = new LoginHandler(repo.Object, jwt.Object);

        var result = await handler.Handle(
            new LoginCommand("test", password),
            CancellationToken.None);

        result.AccessToken.Should().Be("token");
    }


}