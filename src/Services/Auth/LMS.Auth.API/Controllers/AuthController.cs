using System.Security.Claims;
using Application.DTOs;
using AuthService.Application.Commands.ChangePassword;
using AuthService.Application.Commands.ForgotPassword;
using AuthService.Application.Commands.Login;
using AuthService.Application.Commands.RefreshToken;
using AuthService.Application.Commands.RegisterUser;
using AuthService.Application.Commands.ResendVerificationEmail;
using AuthService.Application.Commands.ResetPassword;
using AuthService.Application.Commands.VerifyEmail;
using AuthService.Application.DTOs;
using Domain.Enums;
using LMS.Common.Exceptions;
using LMS.Common.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Auth.API.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<RegisterResponseDto>>> Register(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(request.Email, request.Password, request.Role);
        var result = await mediator.Send(command, cancellationToken);

        return Ok(new ApiResponse<RegisterResponseDto>(
            true,
            new RegisterResponseDto(result.UserId, result.EmailVerficationToken),
            "Registration successful. Please check your email to verify your account. "));
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Email, request.Password);
        var result = await mediator.Send(command, cancellationToken);

        return Ok(new ApiResponse<AuthResponseDto>(
            true,
            result,
            "Login successful."));
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> RefreshToken(
        [FromBody] RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RefreshTokenCommand(request.RefreshToken);
        var result = await mediator.Send(command, cancellationToken);

        return Ok(new ApiResponse<AuthResponseDto>(
            true,
            result,
            "Token refreshed successfully."));
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<ActionResult<ApiResponse<object?>>> ChangePassword(
        [FromBody] ChangePasswordRequest request,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            throw new DomainUnauthorizedException("Invalid or missing user identity claim.");
        }

        await mediator.Send(
            new ChangePasswordCommand(userId, request.CurrentPassword, request.NewPassword),
            cancellationToken);

        return Ok(new ApiResponse<object?>(
            true,
            null,
            "Password changed successfully."));
    }

    [HttpPost("forgot-password")]
    public async Task<ActionResult<ApiResponse<object?>>> ForgotPassword(
        [FromBody] ForgotPasswordRequest request,
        CancellationToken cancellationToken)
    {
        await mediator.Send(new ForgotPasswordCommand(request.Email), cancellationToken);

        return Ok(new ApiResponse<object?>(
            true,
            null,
            "If the email exists, a reset link has been sent."));
    }

    [HttpPost("reset-password")]
    public async Task<ActionResult<ApiResponse<object?>>> ResetPassword(
        [FromBody] ResetPasswordRequest request,
        CancellationToken cancellationToken)
    {
        await mediator.Send(new ResetPasswordCommand(request.Token, request.NewPassword), cancellationToken);

        return Ok(new ApiResponse<object?>(
            true,
            null,
            "Password has been reset successfully."));
    }

    [HttpGet("verify-email")]
    public async Task<ActionResult<ApiResponse<object?>>> VerifyEmail(
        [FromQuery] string token,
        CancellationToken cancellationToken)
    {
        await mediator.Send(new VerifyEmailCommand(token), cancellationToken);

        return Ok(new ApiResponse<object?>(
            true,
            null,
            "Email verified successfully."));
    }

    [HttpPost("resend-verification")]
    public async Task<ActionResult<ApiResponse<object?>>> ResendVerificationEmail(
        [FromBody] ResendVerificationRequest request,
        CancellationToken cancellationToken)
    {
        await mediator.Send(new ResendVerificationEmailCommand(request.Email), cancellationToken);

        return Ok(new ApiResponse<object?>(
            true,
            null,
            "Verification email resent successfully."));
    }

    public sealed record RegisterRequest(string Email, string Password, Role Role = Role.Student);

    public sealed record LoginRequest(string Email, string Password);

    public sealed record RefreshTokenRequest(string RefreshToken);

    public sealed record ChangePasswordRequest(string CurrentPassword, string NewPassword);

    public sealed record ForgotPasswordRequest(string Email);

    public sealed record ResetPasswordRequest(string Token, string NewPassword);

    public sealed record ResendVerificationRequest(string Email);
}