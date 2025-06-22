namespace VMTS.Service.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message)
        : base(message) { }
}

public class ForbbidenException : Exception
{
    public ForbbidenException(string message)
        : base(message) { }
}

public class NotFoundException : Exception
{
    public NotFoundException(string message)
        : base(message) { }
}

public class ConflictException : Exception
{
    public ConflictException(string message)
        : base(message) { }
}
