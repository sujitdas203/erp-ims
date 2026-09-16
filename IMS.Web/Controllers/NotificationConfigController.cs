using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IMS.Helpers.Constants;
using IMS.Models.Notification;
using IMS.Services.Interfaces;

namespace IMS.Web.Controllers
{
    [Authorize]
    public class NotificationConfigController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly IEmailService _emailService;
        private readonly ILogger<NotificationConfigController> _logger;

        public NotificationConfigController(
            INotificationService notificationService,
            IEmailService emailService,
            ILogger<NotificationConfigController> logger)
        {
            _notificationService = notificationService;
            _emailService = emailService;
            _logger = logger;
        }

        private Guid CurrentTenantId
        {
            get
            {
                var raw = User.FindFirst("tenant_id")?.Value;
                if (Guid.TryParse(raw, out var id) && id != Guid.Empty) return id;
                return HardcodedMasterData.CurrentTenantId;
            }
        }

        private string CurrentUserName => User.Identity?.Name ?? "Admin";

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var config = await _notificationService.GetConfigAsync(CurrentTenantId);
            return View(config);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromForm] NotificationConfigModel model)
        {
            model.TenantId = CurrentTenantId;
            var success = await _notificationService.SaveConfigAsync(CurrentTenantId, model);

            if (success)
            {
                TempData["SuccessMessage"] = "Notification settings and event triggers successfully updated!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to save notification settings. Please check your inputs.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> SaveAjax([FromBody] NotificationConfigModel model)
        {
            if (model == null) return BadRequest(new { success = false, message = "Invalid data payload." });

            model.TenantId = CurrentTenantId;
            var success = await _notificationService.SaveConfigAsync(CurrentTenantId, model);

            return Json(new
            {
                success,
                message = success ? "Notification configuration saved successfully." : "Failed to save configuration."
            });
        }

        [HttpPost]
        public async Task<IActionResult> SendTestEmail([FromBody] SendTestEmailRequest request)
        {
            if (request == null) return BadRequest(new { success = false, message = "Invalid email request." });

            var result = await _emailService.SendTestEmailAsync(request);
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpGet]
        public async Task<IActionResult> GetHeaderNotifications()
        {
            var userId = User.FindFirst("user_id")?.Value;
            var role = "TENANT_ADMIN";

            var result = await _notificationService.GetHeaderNotificationsAsync(CurrentTenantId, userId, role);
            return Json(new
            {
                success = true,
                unreadCount = result.UnreadCount,
                items = result.Notifications
            });
        }

        [HttpPost]
        public async Task<IActionResult> MarkRead(Guid id)
        {
            var success = await _notificationService.MarkAsReadAsync(id);
            return Json(new { success });
        }

        [HttpPost]
        public async Task<IActionResult> MarkAllRead()
        {
            var userId = User.FindFirst("user_id")?.Value;
            var role = "TENANT_ADMIN";
            var success = await _notificationService.MarkAllAsReadAsync(CurrentTenantId, userId, role);
            return Json(new { success });
        }

        [HttpPost]
        public async Task<IActionResult> TriggerSample([FromQuery] string eventCode)
        {
            var title = "Sample Alert: " + eventCode;
            var message = "This is a test notification generated from Notification Configuration to verify real-time event triggers.";
            var link = "/NotificationConfig";

            switch (eventCode?.ToUpperInvariant())
            {
                case "ADMISSION_SUBMITTED":
                    title = "New Admission Application #APP-2026-9041";
                    message = "Applicant Rahul Verma submitted admission form for Class XI Science.";
                    link = "/AdmissionApplication";
                    break;
                case "FEE_RECEIVED":
                    title = "Fee Payment Received (₹18,500)";
                    message = "Payment received from Roll #1034 for Term 2 tuition & lab fees.";
                    link = "/Fees";
                    break;
                case "TEACHER_LEAVE_APPLIED":
                    title = "Teacher Leave Request: Prof. K. Roy";
                    message = "Leave application submitted for 3 days starting 20th September.";
                    link = "/TeacherLeave";
                    break;
                case "STUDENT_LEAVE_APPLIED":
                    title = "Student Leave Request: Ankit Sharma (Batch 12-A)";
                    message = "Sick leave application submitted for 2 days. Reason: Viral fever.";
                    link = "/StudentLeave";
                    break;
                case "MOCK_TEST_PUBLISHED":
                    title = "Mock Test Published: Mid-Term Chemistry Test";
                    message = "Mock test scheduled for Batch 11-B on 25th September.";
                    link = "/MockTests";
                    break;
                case "ANNOUNCEMENT_PUBLISHED":
                    title = "Notice: Annual Sports Meet 2026";
                    message = "Annual sports registration opened for all students and faculty.";
                    link = "/Announcements";
                    break;
                case "TC_GENERATED":
                    title = "Transfer Certificate Issued #TC-2026-088";
                    message = "TC document ready for printing for Student Priya Das.";
                    link = "/TransferCertificate";
                    break;
                case "FEE_OVERDUE":
                    title = "Fee Overdue Alert: 18 Defaulters";
                    message = "Term 2 tuition fee past due date for 18 enrolled students.";
                    link = "/Fees";
                    break;
                case "LOW_ATTENDANCE":
                    title = "Low Attendance Alert: Roll #1048";
                    message = "Student attendance dropped below 75% minimum threshold.";
                    link = "/Attendance";
                    break;
            }

            var success = await _notificationService.RaiseNotificationAsync(
                CurrentTenantId,
                eventCode ?? "GENERAL",
                title,
                message,
                link,
                "TENANT_ADMIN"
            );

            return Json(new
            {
                success,
                message = success ? $"Sample notification '{title}' triggered successfully! Check the header bell icon." : "Failed to trigger sample."
            });
        }
    }
}
