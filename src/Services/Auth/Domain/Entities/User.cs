using System;
using Domain.Enums;
using LMS.Common.Entities;

namespace Domain.Entities;

public class User : AuditableEntity
{
    public string Email { get; private set; } = string.Empty; //acts as userName
    public string PasswordHash { get; private set; } = string.Empty;
    public bool IsEmailVerified { get; private set; }
    public string? EmailVerificationToken { get; private set; }
    public string? PasswordResetToken { get; private set; }
    public DateTime? PasswordResetTokenExpiry { get; private set; }
    public string? RefreshToken { get; private set; }
    public DateTime? RefreshTokenExpiry { get; private set; }
    public Role Role { get; private set; }

    private User()
    {
    }

    private User(string email, string passwordHash, Role role, string emailVerificationToken)
    {
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        IsEmailVerified = false;
        EmailVerificationToken = emailVerificationToken;
    }

    public static User Create(string email, string passwordHash, Role role, string emailVerificationToken)
    {
        return new User(email, passwordHash, role, emailVerificationToken);
    }

    public void SetRefreshToken(string refreshToken, DateTime refreshTokenExpiry)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpiry = refreshTokenExpiry;
    }

    public void ClearRefreshToken()
    {
        RefreshToken = null;
        RefreshTokenExpiry = null;
    }

    public void SetPasswordResetToken(string token, DateTime expiry)
    {
        PasswordResetToken = token;
        PasswordResetTokenExpiry = expiry;
    }

    public void ClearPasswordResetToken()
    {
        PasswordResetToken = null;
        PasswordResetTokenExpiry = null;
    }

    public void SetEmailVerificationToken(string token)
    {
        EmailVerificationToken = token;
    }

    public void VerifyEmail()
    {
        IsEmailVerified = true;
        EmailVerificationToken = null;
    }

    public void ChangePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
    }

}
