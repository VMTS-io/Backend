namespace VMTS.API.Errors;

public class ApiErrorValidationResponse : ApiResponse
{
    public IEnumerable<string> Errors { get; set; }
    public ApiErrorValidationResponse() : base(400)
    {
        Errors = new List<string>();
    }
}