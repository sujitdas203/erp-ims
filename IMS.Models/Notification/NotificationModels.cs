using System;
using System.Collections.Generic;

namespace IMS.Models.Notification
{
    public class NotificationConfigModel
    {
        public Guid TenantId { get; set; }
        public bool InAppEnabled { get; set; } = true;
        public bool EmailEnabled { get; set; } = false;

        // SMTP Settings
        public string SmtpHost { get; set; } = "smtp.gmail.com";
        public int SmtpPort { get; set; } = 587;
        public string SmtpUsername { get; set; } = "";
        public string SmtpPassword { get; set; } = "";
        public string FromEmail { get; set; } = "noreply@ims-erp.com";
        public string FromName { get; set; } = "IMS ERP System";
        public bool EnableSsl { get; set; } = true;

        // Event Toggles
        public List<NotificationEventSetting> Events { get; set; } = new List<NotificationEventSetting>();
    }

    public class NotificationEventSetting
    {
        public string EventCode { get; set; } = "";
        public string EventName { get; set; } = "";
        public string Category { get; set; } = "General";
        public string Description { get; set; } = "";
        public bool InAppEnabled { get; set; } = true;
        public bool EmailEnabled { get; set; } = false;
        public string IconClass { get; set; } = "fa-solid fa-bell";
        public string ColorClass { get; set; } = "primary";
    }

    public class UserNotification
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid TenantId { get; set; }
        public string? UserId { get; set; }
        public string? TargetRole { get; set; } = "TENANT_ADMIN";
        public string Title { get; set; } = "";
        public string Message { get; set; } = "";
        public string EventType { get; set; } = "";
        public string? LinkUrl { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? IconClass { get; set; } = "fa-solid fa-bell";
        public string? BadgeColor { get; set; } = "primary";
        public string TimeAgo { get; set; } = "Just now";
    }

    public class SendTestEmailRequest
    {
        public string ToEmail { get; set; } = "";
        public string SmtpHost { get; set; } = "";
        public int SmtpPort { get; set; } = 587;
        public string SmtpUsername { get; set; } = "";
        public string SmtpPassword { get; set; } = "";
        public string FromEmail { get; set; } = "";
        public string FromName { get; set; } = "";
        public bool EnableSsl { get; set; } = true;
    }

    public class HeaderNotificationResult
    {
        public int UnreadCount { get; set; }
        public List<UserNotification> Notifications { get; set; } = new List<UserNotification>();
    }
}
