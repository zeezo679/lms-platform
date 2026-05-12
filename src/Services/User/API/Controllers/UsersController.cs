using Application.Commands.DeactivateUser;
using Application.Commands.DeleteUser;
using Application.Commands.UpdateUserAvatar;
using Application.Commands.UpdateUserProfile;
using Application.Dtos;
using Application.Queries.GetAllUsers;
using Application.Queries.GetUserByEmail;
using Application.Queries.GetUserById;
using LMS.Common.Exceptions;
using LMS.Common.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/users")]
    public class UsersController(IMediator mediator) : AppBaseController
    {
        private Guid GetCallerAuthUserId()
        {
            var value = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(value) || !Guid.TryParse(value, out var id))
                throw new DomainUnauthorizedException("Invalid or missing user identity claim.");

            return id;
        }

        private string GetCallerEmail()
        {
            var value = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrWhiteSpace(value))
                throw new DomainUnauthorizedException("Invalid or missing user email claim.");

            return value;
        }

        // GET /api/users
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<ApiResponse<PagedResponse<UserSummaryDto>>>> GetAllUsers(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken ct = default)
        {
            var query = new GetAllUsersQuery(pageNumber, pageSize);
            var result = await mediator.Send(query, ct);
            return Success(result, "Users retrieved successfully.");
        }

        // GET /api/users/me
        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<UserProfileDto>>> GetMyProfile(CancellationToken ct)
        {
            var query = new GetUserByEmailQuery(GetCallerEmail());

            var result = await mediator.Send(query, ct);
            return Success(result, "Profile retrieved successfully.");
        }

        // GET /api/users/{id}
        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<UserProfileDto>>> GetUserById(Guid id, CancellationToken ct)
        {
            var query = new GetUserByIdQuery(id);
            var result = await mediator.Send(query, ct);
            return Success(result, "User retrieved successfully.");
        }

        // PUT /api/users/me
        [HttpPut("me")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<UserProfileDto>>> UpdateMyProfile(
            [FromBody] UpdateUserProfileRequest request, CancellationToken ct)
        {
            var authUserId = GetCallerAuthUserId();

            var command = new UpdateUserProfileCommand(
                authUserId,
                request.FirstName,
                request.LastName,
                request.Bio,
                request.AvatarUrl,
                request.PhoneNumber);

            var result = await mediator.Send(command, ct);
            return Success(result, "Profile updated successfully.");
        }

        // PUT /api/users/{authUserId}/deactivate
        [HttpPut("{authUserId:guid}/deactivate")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<string>>> DeactivateUser(Guid authUserId, CancellationToken ct)
        {
            var command = new DeactivateUserCommand(authUserId);
            await mediator.Send(command, ct);
            return Success("User deactivated successfully.");
        }

        // DELETE /api/users/{authUserId}
        [HttpDelete("{authUserId:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteUser(Guid authUserId, CancellationToken ct)
        {
            var command = new DeleteUserCommand(authUserId);
            await mediator.Send(command, ct);
            return Success("User deleted successfully.");
        }

        [HttpPut("me/avatar")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<string>>> UpdateUserAvatar(
            [FromBody] UpdateAvatarDto updateAvatarDto,
            CancellationToken ct)
        {
            var authUserId = GetCallerAuthUserId();

            var command = new UpdateUserAvatarCommand(authUserId, updateAvatarDto.AvatarUrl);
            await mediator.Send(command, ct);
            return Success("Avatar updated successfully.");
        }
    }
}
