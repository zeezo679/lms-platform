namespace LMS.Common.Exceptions;

public class DomainConflictException : Exception
{
    public DomainConflictException(string message)
        : base(message)
    {
    }

    public DomainConflictException()
        : base("A conflict occurred while processing the request.")
    {
    }
}