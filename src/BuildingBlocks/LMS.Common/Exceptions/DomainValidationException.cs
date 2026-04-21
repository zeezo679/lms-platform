using System;

namespace LMS.Common.Exceptions;

public class DomainValidationException : Exception
{
    public DomainValidationException(string message)
        : base(message) { }

    public DomainValidationException(string objectName, string validationMessage)
        : base($"{objectName} validation failed: {validationMessage}") { }

    public DomainValidationException() : base("Domain validation failed.") { }
}
