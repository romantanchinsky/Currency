using System.Security.Authentication;
using MediatR;
using User.Application.DTOs;
using User.Application.Exceptions;
using User.Application.Interfaces;

namespace User.Application.Commands.LoginUser;

public class LoginHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IUserRepository _repo;
    private readonly IJwtTokenGenerator _jwt;

    public LoginHandler(IUserRepository repo, IJwtTokenGenerator jwt)
    {
        _repo = repo;
        _jwt = jwt;
    }

    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await _repo.GetByNameAsync(request.Name, ct)
            ?? throw new InvalidCredentialsException();

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new InvalidCredentialsException();

        var token = _jwt.GenerateToken(user.Id, user.Name);

        return new AuthResponse(token);
    }
}
