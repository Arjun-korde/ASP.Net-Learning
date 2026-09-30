using System;

namespace server.Exceptions;

public class NotFoundException : AppException
{
    public NotFoundException(string message)
        : base(message)
    {
    }
}
