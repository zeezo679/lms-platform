using System;

namespace LMS.Common.Exceptions;

public class DomainNotFoundException : Exception
{
    public DomainNotFoundException(string objectName, object key)
        : base($"{objectName} with key '{key}' was not found.") { }

    public DomainNotFoundException(string message)
        : base(message) { }

    public DomainNotFoundException() : base("The requested resource was not found.") { }
}
