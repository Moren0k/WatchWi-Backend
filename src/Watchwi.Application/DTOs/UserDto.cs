namespace Watchwi.Application.DTOs;

public record UserDto(
    Guid Id,
    string Username,
    string Email,
    string Role,
    bool Status,
    string Plan,
    string? ProfileImageUrl
);