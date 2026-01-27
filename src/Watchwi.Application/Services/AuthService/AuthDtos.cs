using Watchwi.Application.DTOs;

namespace Watchwi.Application.Services.AuthService;

public record LoginRequestDto(
    string Email,
    string Password
);

public record RegisterRequestDto(
    string Username,
    string Email,
    string Password
);

public record AuthResponseDto(
    string AccessToken,
    UserDto User
);