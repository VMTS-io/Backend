using System.Threading.Tasks;

namespace VMTS.Core.Interfaces.Services;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml = true);
}
