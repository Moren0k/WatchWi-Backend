using Watchwi.Application.DTOs;
using Watchwi.Application.IProviders.Security;
using Watchwi.Domain.Common.IRepositories;
using Watchwi.Domain.Entities.Users;

namespace Watchwi.Application.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(
        IUserRepository userRepository,
        IJwtProvider jwtProvider,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto loginRequest)
    {
        var user = await _userRepository.GetByEmailAsync(loginRequest.Email);
        if (user == null)
        {
            return null;
        }
        
        var isValidPassword = _passwordHasher.Verify(loginRequest.Password, user.PasswordHash);

        if (!isValidPassword)
        {
            return null;
        }

        var userDto = new UserDto(
            user.Id,
            user.Username,
            user.Email,
            user.Role.ToString(),
            user.Status,
            user.Plan.ToString(),
            user.ProfileImage?.Url
        );
        
        var userToken = _jwtProvider.Generate(userDto);
        
        return new AuthResponseDto(
            userToken,
            userDto
        );
    }

    public async Task<AuthResponseDto?> RegisterAsync(RegisterRequestDto registerRequest)
    {
        var availabilityEmail = await _userRepository.GetByEmailAsync(registerRequest.Email);
        var availabilityUsername = await _userRepository.GetByUsernameAsync(registerRequest.Email);

        if (availabilityEmail != null || availabilityUsername != null)
        {
            return null;
        }

        var passwordHash = _passwordHasher.Hash(registerRequest.Password);

        var newUser = new User(
            registerRequest.Username,
            registerRequest.Email,
            passwordHash);

        _userRepository.Add(newUser);
        await _unitOfWork.SaveChangesAsync();

        var userDto = new UserDto(
            newUser.Id,
            newUser.Username,
            newUser.Email,
            newUser.Role.ToString(),
            newUser.Status,
            newUser.Plan.ToString(),
            newUser.ProfileImage?.Url
        );

        var userToken = _jwtProvider.Generate(userDto);

        return new AuthResponseDto(
            userToken,
            userDto
        );
    }
}