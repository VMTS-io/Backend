namespace VMTS.Service.Exceptions;

public class HttpErrorException : Exception
{
    public HttpErrorException(int statusCode, string title, string message)
        : base(message)
    {
        StatusCode = statusCode;
        Title = title;
    }

    public string Title { get; set; } = default!;
    public int StatusCode { get; set; }
}

public class BadRequestException : HttpErrorException
{
    public BadRequestException(string message)
        : base(400, "Bad Request", message) { }
}

public class ForbbidenException : HttpErrorException
{
    public ForbbidenException(string message)
        : base(403, "Forbbiden", message) { }
}

public class NotFoundException : HttpErrorException
{
    public NotFoundException(string message)
        : base(404, "Not Found", message) { }
}

public class ConflictException : HttpErrorException
{
    public ConflictException(string message)
        : base(409, "Conflict", message) { }
}
