namespace Watchwi.Application.DTOs;

public record UserDto(
    string Id,
    string Username,
    string Email,
    string Role,
    string Status,
    string Plan
);