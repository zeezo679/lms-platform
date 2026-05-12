using Domain.Enums;
using LMS.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserProfile: AuditableEntity
    {
        public Guid AuthUserId { get; private set;  }
        public string Email { get; private set; } = string.Empty;
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set;} = string.Empty;
        public string? Bio {  get; private set; }
        public string? AvatarUrl { get; private set; }
        public string? PhoneNumber { get; private set; }
        public UserRole Role { get; private set; }
        public UserStatus Status { get; private set; } = UserStatus.Active;
        public string FullName => $"{FirstName} {LastName}".Trim();

        private UserProfile() { }
        
        private UserProfile(
            Guid authUserId,
            string email,
            string firstName,
            string lastName,
            UserRole role)
        {
            AuthUserId = authUserId;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Role = role;
            Status = UserStatus.Active;
        }

        public static UserProfile Create(
            Guid authUserId,
            string email,
            string firstName,
            string lastName,
            UserRole role)
        {
            return new UserProfile(authUserId, email, firstName, lastName, role);
        }

        public void UpdateProfile(
            string firstName,
            string lastName,
            string? bio,
            string? avatarUrl,
            string? phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            Bio = bio;
            AvatarUrl = avatarUrl;
            PhoneNumber = phoneNumber;
        }

        public void UpdateEmail(string email)
        {
            Email = email;
        }

        public void UpdateAvatar(string avatarUrl)
        {
            AvatarUrl = avatarUrl;
        }

        public void Deactivate()
        {
            Status = UserStatus.Inactive;
        }

        public void Activate()
        {
            Status = UserStatus.Active;
        }
    }
}
