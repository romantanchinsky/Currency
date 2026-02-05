using MediatR;
using User.Application.DTOs;

namespace User.Application.Commands.RegisterUser;

public record RegisterUserCommand(string Name, string Password)
    : IRequest<AuthResponse>;
