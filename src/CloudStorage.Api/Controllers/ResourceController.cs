using Asp.Versioning;
using CloudStorage.Api.Models;
using CloudStorage.Application.Resources.CompleteUpload;
using CloudStorage.Application.Resources.CreateFolder;
using CloudStorage.Application.Resources.CreateUploadUrl;
using CloudStorage.Application.Resources.ListResources;
using CloudStorage.Application.Resources.ListTrash;
using CloudStorage.Application.Resources.RestoreResource;
using CloudStorage.Application.Resources.SoftDeleteResource;
using CloudStorage.Application.Resources.UpdateResource;
using CommunityToolkit.HighPerformance.Helpers;
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
        [FromBody] CreateFolderCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("upload-url")]
    public async Task<IActionResult> CreateUploadUrl(
        [FromBody] CreateUploadUrlCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateResource(
        Guid id,
        [FromBody] UpdateResourceRequest request,
        CancellationToken cancellationToken = default)
    {
        var result =
            await mediator.Send(new UpdateResourceCommand(id, request.Name),
                cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:guid}/complete-upload")]
    public async Task<IActionResult> CompleteUpload(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        await mediator.Send(new CompleteUploadCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> SoftDeleteResource(Guid id, CancellationToken cancellationToken = default)
    {
        await mediator.Send(new SoftDeleteResourceCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/restore")]
    public async Task<IActionResult> RestoreResource(Guid id, CancellationToken cancellationToken = default)
    {
        await mediator.Send(new RestoreResourceCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpGet("trash")]
    public async Task<IActionResult> ListTrash(CancellationToken cancellationToken = default)
    {
        var resources = await mediator.Send(new ListTrashQuery(), cancellationToken);
        return Ok(resources);
    }

    [HttpDelete("trash")]
    public async Task<IActionResult> EmptyTrash(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Method not implemented.");
    }
}