using System;

namespace server.Exceptions;

public class ConflictException : AppException
{
    public ConflictException(string message)
        : base(message)
    {
    }
}
