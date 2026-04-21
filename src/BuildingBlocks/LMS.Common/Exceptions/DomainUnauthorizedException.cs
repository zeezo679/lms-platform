using System;

namespace LMS.Common.Exceptions;

public class DomainUnauthorizedException : Exception
{
    public DomainUnauthorizedException(string message)
        : base(message) { }

    public DomainUnauthorizedException() : base("Unauthorized access.") { }
}
