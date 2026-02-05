using MediatR;
using Microsoft.AspNetCore.Mvc;
using User.Application.Commands.LoginUser;
using User.Application.Commands.RegisterUser;
using User.Application.DTOs;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<AuthResponse> Register(RegisterRequest req)
        => await _mediator.Send(new RegisterUserCommand(req.Name, req.Password));

    [HttpPost("login")]
    public async Task<AuthResponse> Login(LoginRequest req)
        => await _mediator.Send(new LoginCommand(req.Name, req.Password));
}
