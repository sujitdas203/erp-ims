using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using IMS.Models.Notification;
using IMS.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace IMS.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public async Task<(bool Success, string Message)> SendEmailAsync(
            string toEmail, string subject, string bodyHtml, NotificationConfigModel? smtpConfig = null)
        {
            if (string.IsNullOrWhiteSpace(toEmail))
                return (false, "Recipient email address is missing.");

            if (smtpConfig == null || string.IsNullOrWhiteSpace(smtpConfig.SmtpHost))
                return (false, "SMTP host is not configured.");

            try
            {
                using var client = new SmtpClient(smtpConfig.SmtpHost, smtpConfig.SmtpPort)
                {
                    EnableSsl = smtpConfig.EnableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Timeout = 15000
                };

                if (!string.IsNullOrWhiteSpace(smtpConfig.SmtpUsername))
                {
                    client.Credentials = new NetworkCredential(smtpConfig.SmtpUsername, smtpConfig.SmtpPassword ?? "");
                }

                var from = new MailAddress(
                    string.IsNullOrWhiteSpace(smtpConfig.FromEmail) ? "noreply@ims-erp.com" : smtpConfig.FromEmail,
                    string.IsNullOrWhiteSpace(smtpConfig.FromName) ? "IMS ERP" : smtpConfig.FromName
                );

                using var mailMessage = new MailMessage
                {
                    From = from,
                    Subject = subject,
                    Body = bodyHtml,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage);
                return (true, "Email sent successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {ToEmail}", toEmail);
                return (false, $"SMTP error: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> SendTestEmailAsync(SendTestEmailRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ToEmail))
                return (false, "Please provide a valid test recipient email address.");

            if (string.IsNullOrWhiteSpace(request.SmtpHost))
                return (false, "SMTP Host is required.");

            try
            {
                using var client = new SmtpClient(request.SmtpHost, request.SmtpPort)
                {
                    EnableSsl = request.EnableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Timeout = 12000
                };

                if (!string.IsNullOrWhiteSpace(request.SmtpUsername))
                {
                    client.Credentials = new NetworkCredential(request.SmtpUsername, request.SmtpPassword ?? "");
                }

                var from = new MailAddress(
                    string.IsNullOrWhiteSpace(request.FromEmail) ? "noreply@ims-erp.com" : request.FromEmail,
                    string.IsNullOrWhiteSpace(request.FromName) ? "IMS ERP Notifications" : request.FromName
                );

                using var mailMessage = new MailMessage
                {
                    From = from,
                    Subject = "[IMS ERP] Test Notification Email Configuration",
                    Body = $@"
<!DOCTYPE html>
<html>
<head>
<meta charset='utf-8' />
<style>
    body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background: #f8fafc; margin: 0; padding: 20px; }}
    .card {{ max-width: 540px; margin: 0 auto; background: #ffffff; border-radius: 12px; border: 1px solid #e2e8f0; overflow: hidden; box-shadow: 0 4px 12px rgba(0,0,0,0.05); }}
    .header {{ background: #1e293b; color: #ffffff; padding: 20px 24px; text-align: center; }}
    .content {{ padding: 24px; color: #334155; line-height: 1.6; }}
    .badge {{ display: inline-block; background: #dcfce7; color: #15803d; font-weight: 600; padding: 4px 12px; border-radius: 999px; font-size: 13px; margin-bottom: 12px; }}
    .footer {{ background: #f1f5f9; padding: 14px 24px; text-align: center; font-size: 12px; color: #64748b; }}
</style>
</head>
<body>
    <div class='card'>
        <div class='header'>
            <h2 style='margin:0; font-size: 20px;'>IMS Notification Server</h2>
        </div>
        <div class='content'>
            <span class='badge'>&#10004; Configuration Verified</span>
            <h3 style='margin: 0 0 10px 0; color: #0f172a;'>SMTP Test Successful!</h3>
            <p>Congratulations! Your SMTP server settings have been tested and verified. Automated transactional notifications and administrative alerts can now be dispatched from your IMS ERP application.</p>
            <p style='font-size: 13px; color: #64748b;'><strong>Timestamp:</strong> {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC</p>
        </div>
        <div class='footer'>
            &copy; {DateTime.UtcNow.Year} IMS ERP Platform. All rights reserved.
        </div>
    </div>
</body>
</html>",
                    IsBodyHtml = true
                };

                mailMessage.To.Add(request.ToEmail);

                await client.SendMailAsync(mailMessage);
                return (true, $"Test email successfully delivered to {request.ToEmail}!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Test email dispatch failed to {ToEmail}", request.ToEmail);
                return (false, $"Connection/Authentication failed: {ex.Message}");
            }
        }
    }
}
