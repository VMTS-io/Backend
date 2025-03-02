namespace VMTS.API.Errors;

public class ApiResponse
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }

    public ApiResponse(int statusCode, string? message = null)
    {
        StatusCode = statusCode;
        Message = message ?? GetDefaultErrorMessage(statusCode);
    }

    private string? GetDefaultErrorMessage(int statusCode)
    {
        return statusCode switch
        {
            400 => "a bad request , you have made ",
            401 => "Authorized you are not",
            404 => "Resource was not found",
            500 =>
                "Errors are the path to the dark side. Errors leads to Anger. Anger leads to hate. Hate leads to change career."
        };
    }
}