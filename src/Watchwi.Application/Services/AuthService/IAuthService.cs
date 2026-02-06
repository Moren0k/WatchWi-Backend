namespace Watchwi.Application.Services.AuthService;

public interface IAuthService
{
    public Task<AuthResponseDto?> LoginAsync(LoginRequestDto loginRequest);
    public Task<AuthResponseDto?> RegisterAsync(RegisterRequestDto registerRequest);
}