using AuthService.Application.DTOs;
using Domain.Enums;
using MediatR;

namespace AuthService.Application.Commands.RegisterUser;

public sealed record RegisterUserCommand(string Email, string Password, Role Role = Role.Student) : IRequest<AuthResponseDto>;
