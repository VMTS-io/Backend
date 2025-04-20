namespace VMTS.API.Errors;

public class ApiErrorValidationResponse : ApiErrorResponse
{
    public Dictionary<string, List<string>> Errors { get; set; }

    public ApiErrorValidationResponse()
        : base(400)
    {
        Errors = [];
    }
}
