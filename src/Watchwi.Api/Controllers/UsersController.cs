using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Watchwi.Application.DTOs;
using Watchwi.Application.Services.UserService;

namespace Watchwi.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    private Guid GetUserId()
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier) 
                    ?? User.Claims.FirstOrDefault(c => c.Type == "sub");
        
        if (claim == null || !Guid.TryParse(claim.Value, out var id))
            throw new UnauthorizedAccessException("Invalid User ID in token");
            
        return id;
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserRequestDto request)
    {
        var result = await _userService.UpdateProfileAsync(GetUserId(), request);
        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpPost("profile-image")]
    public async Task<IActionResult> UploadProfileImage([FromForm] IFormFile file)
    {
        if (file == null) return BadRequest("File is required");

        using var stream = file.OpenReadStream();
        var fileRequest = new FileUploadRequest(
            stream,
            file.FileName,
            file.ContentType,
            file.Length
        );

        var result = await _userService.UploadProfileImageAsync(GetUserId(), new UploadProfileImageRequestDto(fileRequest));
         if (!result.IsSuccess)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpGet("favorites")]
    public async Task<IActionResult> GetFavorites()
    {
        var result = await _userService.GetFavoritesAsync(GetUserId());
        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpPost("favorites/{mediaId:guid}")]
    public async Task<IActionResult> AddFavorite(Guid mediaId)
    {
        var result = await _userService.AddFavoriteAsync(GetUserId(), mediaId);
        if (!result.IsSuccess)
        {
            if (result.Error == "Media not found") return NotFound(result.Error);
            return BadRequest(result.Error);
        }

        return Ok("Added to favorites");
    }

    [HttpDelete("favorites/{mediaId:guid}")]
    public async Task<IActionResult> RemoveFavorite(Guid mediaId)
    {
        var result = await _userService.RemoveFavoriteAsync(GetUserId(), mediaId);
        if (!result.IsSuccess)
             return BadRequest(result.Error);

        return Ok("Removed from favorites");
    }
}
