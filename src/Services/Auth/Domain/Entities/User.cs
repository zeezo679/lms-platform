using System;
using Domain.Enums;
using LMS.Common.Entities;

namespace Domain.Entities;

public class User : AuditableEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public Roles Role { get; private set; }

}
