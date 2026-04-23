using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using Domain.Entities;
using LMS.Common.Exceptions;
using LMS.Contracts.Events;
using LMS.EventBus.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Application.Commands.RegisterUser;

public sealed class RegisterUserCommandHandler(
    IAppDbContext context,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator,
    IEventBus eventBus) : IRequestHandler<RegisterUserCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var emailExists = await context.Users
            .AnyAsync(u => u.Email == normalizedEmail, cancellationToken);

        if (emailExists)
            throw new DomainConflictException($"A user with email '{normalizedEmail}' already exists.");
        

        var passwordHash = passwordHasher.Hash(request.Password);
        var verificationToken = Guid.NewGuid().ToString();

        var user = User.Create(normalizedEmail, passwordHash, request.Role, verificationToken);

        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        await eventBus.PublishAsync(
            new UserRegisteredIntegrationEvent(
                user.Id,
                user.Email,
                verificationToken,
                user.Role.ToString()),
            cancellationToken);

        var accessToken = jwtTokenGenerator.GenerateAccessToken(user);
        var refreshToken = jwtTokenGenerator.GenerateRefreshToken();

        user.SetRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));
        await context.SaveChangesAsync(cancellationToken);

        return new AuthResponseDto(accessToken, refreshToken, user.Id);
    }
}
