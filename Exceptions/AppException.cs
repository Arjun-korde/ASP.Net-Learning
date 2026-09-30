using System;

namespace server.Exceptions;

public class AppException : Exception
{
     protected AppException(string message)
        : base(message)
    {
    }
}
