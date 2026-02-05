using FluentAssertions;
using Moq;
using User.Application.Commands.RegisterUser;
using User.Application.Exceptions;
using User.Application.Interfaces;
using User.Domain.Entities;

public class RegisterUserHandlerTests
{
    [Fact]
    public async Task RegisterUser_WhenUserDoesNotExist_ReturnsToken()
    {
        var repo = new Mock<IUserRepository>();
        repo.Setup(r => r.GetByNameAsync("test", It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserAccount?)null);

        var jwt = new Mock<IJwtTokenGenerator>();
        jwt.Setup(j => j.GenerateToken(It.IsAny<Guid>(), "test"))
            .Returns("token");

        var handler = new RegisterUserHandler(repo.Object, jwt.Object);

        var result = await handler.Handle(
            new RegisterUserCommand("test", "123"),
            CancellationToken.None);

        result.AccessToken.Should().Be("token");
        repo.Verify(r => r.AddAsync(It.IsAny<UserAccount>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RegisterUser_WhenUserExists_ThrowsException()
    {
        var repo = new Mock<IUserRepository>();
        repo.Setup(r => r.GetByNameAsync("test", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserAccount
            {
                Id = Guid.NewGuid(),
                Name = "test",
                PasswordHash = "hash"
            });

        var handler = new RegisterUserHandler(
            repo.Object,
            Mock.Of<IJwtTokenGenerator>());

        Func<Task> act = async () =>
            await handler.Handle(
                new RegisterUserCommand("test", "123"),
                CancellationToken.None);

        await act.Should().ThrowAsync<UserAlreadyExistsException>();
    }

}
