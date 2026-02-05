using System.Security.Authentication;
using MediatR;
using User.Application.DTOs;
using User.Application.Exceptions;
using User.Application.Interfaces;
using User.Domain.Entities;

namespace User.Application.Commands.RegisterUser;

public class RegisterUserHandler
    : IRequestHandler<RegisterUserCommand, AuthResponse>
{
    private readonly IUserRepository _repo;
    private readonly IJwtTokenGenerator _jwt;

    public RegisterUserHandler(IUserRepository repo, IJwtTokenGenerator jwt)
    {
        _repo = repo;
        _jwt = jwt;
    }

    public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken ct)
    {
        if (await _repo.GetByNameAsync(request.Name, ct) != null)
            throw new UserAlreadyExistsException(request.Name);

        var user = new UserAccount
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        await _repo.AddAsync(user, ct);

        return new AuthResponse(_jwt.GenerateToken(user.Id, user.Name));
    }
}
