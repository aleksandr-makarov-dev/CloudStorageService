using Asp.Versioning;
using CloudStorage.Models;
using CloudStorage.Services;
using Microsoft.AspNetCore.Mvc;

namespace CloudStorage.Controllers;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/resources")]
public class ResourceController(IResourceService resourceService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ListResources(
        [FromQuery] ListResourcesQueryParams query,
        CancellationToken cancellationToken = default)
    {
        var result = await resourceService.ListAsync(query, cancellationToken);

        return Ok(result);
    }

    [HttpPost("folder")]
    public async Task<IActionResult> CreateFolder(
        [FromBody] CreateFolderRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await resourceService.CreateFolderAsync(request, cancellationToken);

        return Ok(result);
    }

    [HttpPost("upload-url")]
    public async Task<IActionResult> CreateUploadUrl(
        [FromBody] CreateUploadUrlRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await resourceService.CreateUploadUrlAsync(request, cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:guid}/complete-upload")]
    public async Task<IActionResult> CompleteUpload(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        await resourceService.CompleteUploadAsync(id, cancellationToken);

        return NoContent();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateResource(
        Guid id,
        [FromBody] UpdateResourceRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await resourceService.UpdateAsync(id, request, cancellationToken);

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteResource(Guid id, CancellationToken cancellationToken = default)
    {
        await resourceService.DeleteAsync(id, cancellationToken);
        
        return NoContent();
    }
}