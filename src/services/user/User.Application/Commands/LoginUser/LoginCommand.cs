using MediatR;
using User.Application.DTOs;

namespace User.Application.Commands.LoginUser;

public record LoginCommand(string Name, string Password) : IRequest<AuthResponse>;
