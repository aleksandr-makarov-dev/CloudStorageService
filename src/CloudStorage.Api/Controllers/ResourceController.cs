using Asp.Versioning;
using CloudStorage.Api.Models;
using CloudStorage.Application.Resources.CompleteUpload;
using CloudStorage.Application.Resources.CreateFolder;
using CloudStorage.Application.Resources.CreateUploadUrl;
using CloudStorage.Application.Resources.ListResources;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace CloudStorage.Api.Controllers;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/resources")]
public class ResourceController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ListResources(
        [FromQuery] ListResourcesQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost("folder")]
    public async Task<IActionResult> CreateFolder(
        [FromBody] CreateFolderRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("upload-url")]
    public async Task<IActionResult> CreateUploadUrl(
        [FromBody] CreateUploadUrlRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(request, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:guid}/complete-upload")]
    public async Task<IActionResult> CompleteUpload(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        await mediator.Send(new CompleteUploadRequest(id), cancellationToken);
        return NoContent();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateResource(
        Guid id,
        [FromBody] UpdateResourceRequest request,
        CancellationToken cancellationToken = default)
    {
        var result =
            await mediator.Send(new Application.Resources.UpdateResource.UpdateResourceRequest(id, request.Name),
                cancellationToken);
        return Ok(result);
    }
}