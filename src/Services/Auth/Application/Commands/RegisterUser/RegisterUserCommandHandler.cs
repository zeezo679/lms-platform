using Application.DTOs;
using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using Domain.Entities;
using LMS.Common.Exceptions;
using LMS.Contracts.Events;
using LMS.EventBus.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuthService.Application.Commands.RegisterUser;

public sealed class RegisterUserCommandHandler(
    IAppDbContext context,
    IPasswordHasher passwordHasher,
    ILogger<RegisterUserCommandHandler> logger,
    IEventBus eventBus) : IRequestHandler<RegisterUserCommand, RegisterResponseDto>
{
    public async Task<RegisterResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
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


        try
        {
            await eventBus.PublishAsync(
            new UserRegisteredIntegrationEvent(
                user.Id,
                user.Email,
                verificationToken,
                user.Role.ToString()),
            cancellationToken);
        } catch (Exception ex)
        {
            logger.LogError(ex, 
            "User {UserId} was created in the db but failed to publish UserRegisteredIntegrationEvent. Manual intervention may be required to send the verification email.", user.Id);
        }

        return new RegisterResponseDto(user.Id, verificationToken);
    }
}
