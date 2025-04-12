namespace VMTS.API.Errors;

public class ApiErrorValidationResponse : ApiResponse
{
    public Dictionary<string, List<string>> Errors { get; set; }

    public ApiErrorValidationResponse()
        : base(400)
    {
        Errors = [];
    }
}
