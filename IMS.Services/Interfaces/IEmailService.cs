using System.Threading.Tasks;
using IMS.Models.Notification;

namespace IMS.Services.Interfaces
{
    public interface IEmailService
    {
        Task<(bool Success, string Message)> SendEmailAsync(string toEmail, string subject, string bodyHtml, NotificationConfigModel? smtpConfig = null);
        Task<(bool Success, string Message)> SendTestEmailAsync(SendTestEmailRequest request);
    }
}
