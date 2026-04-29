using System;
using System.Collections.Concurrent;
using LMS.Common.Entities;

namespace Domain.Entities;

public class RefreshToken : BaseEntity
{
    public string Token {get; private set; } = string.Empty;
    public Guid UserId { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsRevoked { get; private set; } = false;
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => !IsRevoked && !IsExpired;
    public User user {get; private set; } = null!;

    public RefreshToken(string token, Guid userId, DateTime expiresAt)
    {
        Token = token;
        UserId = userId;
        ExpiresAt = expiresAt;
    }
}
