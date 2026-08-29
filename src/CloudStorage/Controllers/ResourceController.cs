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
    [HttpPost("upload-url")]
    public async Task<IActionResult> CreateUploadUrl([FromBody] CreateUploadUrlRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await resourceService.CreateUploadUrlAsync(request, cancellationToken);

        return Ok(result);
    }
}