using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Watchwi.Api.Dtos;
using Watchwi.Application.Services.MediaService;

namespace Watchwi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class MediaController : ControllerBase
{
    private readonly IMediaService _mediaService;

    public MediaController(IMediaService mediaService)
    {
        _mediaService = mediaService;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create([FromForm] CreateMediaRequest request)
    {
        if (request.Poster.Length == 0)
            return BadRequest("Poster image is required.");

        await using var stream = request.Poster.OpenReadStream();

        var command = new CreateMediaWithPosterCommand(
            request.Title,
            request.Description,
            request.MediaType,
            request.MediaUrl,
            stream,
            request.Poster.FileName,
            request.CategoryIds
        );

        var mediaId = await _mediaService.CreateWithPosterAsync(command);

        return CreatedAtAction(nameof(GetById), new { id = mediaId }, null);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var medias = await _mediaService.GetAllAsync();
        return Ok(medias);
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var media = await _mediaService.GetByIdAsync(id);
        return Ok(media);
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{id:guid}/feature")]
    public async Task<IActionResult> MarkAsFeatured(Guid id)
    {
        await _mediaService.MarkAsFeaturedAsync(id);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediaService.DeleteAsync(id);
        return NoContent();
    }
}