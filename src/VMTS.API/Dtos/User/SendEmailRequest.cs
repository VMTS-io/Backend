namespace VMTS.API.Dtos;

public class SendEmailRequest
{
    public string toEmail { get; set; }
    public string subject { get; set; }
    public string body { get; set; }
    public bool isHtml = true;
}
