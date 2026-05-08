using LMS.Common.Responses;
using LMS.Upload.Application.Commands.UploadFile;
using LMS.Upload.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using LMS.Common.Exceptions;

namespace LMS.Upload.API.Controllers;

[ApiController]
[Route("api/upload")]
[Authorize]
public sealed class UploadController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Uploads a file for a given context (CourseMaterial, AssignmentSubmission, UserProfilePicture).
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<string>>> Upload(
        IFormFile file,
        [FromQuery] UploadContext context,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var command = new UploadFileCommand(file, context, userId);
        var fileUrl = await mediator.Send(command, cancellationToken);

        return Ok(new ApiResponse<string>(
            true,
            fileUrl,
            "File uploaded successfully."));
    }

    private Guid GetCurrentUserId()
    {
        // After YARP with ClaimsToHeadersMiddleware
        // GatewayAuthenticationHandler reconstructs the ClaimsPrincipal from X-User-Id header
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(userIdClaim, out var userId))
            throw new DomainUnauthorizedException("Invalid or missing user identity claim.");

        return userId;
    }
}
