using Watchwi.Application.DTOs;

namespace Watchwi.Application.IProviders.Security;

public interface IJwtProvider
{
    string Generate(UserDto user);
}