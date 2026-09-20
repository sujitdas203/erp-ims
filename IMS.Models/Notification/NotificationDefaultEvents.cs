using System.Collections.Generic;

namespace IMS.Models.Notification
{
    public static class NotificationDefaultEvents
    {
        public static List<NotificationEventSetting> GetDefaultEvents()
        {
            return new List<NotificationEventSetting>
            {
                new NotificationEventSetting
                {
                    EventCode = "ADMISSION_SUBMITTED",
                    EventName = "New Admission Application",
                    Category = "Admissions",
                    Description = "Triggered when a prospective student submits an online admission application.",
                    InAppEnabled = true,
                    EmailEnabled = true,
                    IconClass = "fa-solid fa-address-book",
                    ColorClass = "primary"
                },
                new NotificationEventSetting
                {
                    EventCode = "FEE_RECEIVED",
                    EventName = "Fee Payment Received",
                    Category = "Finance",
                    Description = "Triggered when a fee installment or invoice payment is successfully recorded.",
                    InAppEnabled = true,
                    EmailEnabled = true,
                    IconClass = "fa-solid fa-indian-rupee-sign",
                    ColorClass = "success"
                },
                new NotificationEventSetting
                {
                    EventCode = "FEE_OVERDUE",
                    EventName = "Fee Overdue / Defaulter Alert",
                    Category = "Finance",
                    Description = "Alerts administrators when student dues cross the grace payment window.",
                    InAppEnabled = true,
                    EmailEnabled = false,
                    IconClass = "fa-solid fa-file-invoice-dollar",
                    ColorClass = "danger"
                },
                new NotificationEventSetting
                {
                    EventCode = "TEACHER_LEAVE_APPLIED",
                    EventName = "Teacher Leave Request",
                    Category = "Staff & HR",
                    Description = "Triggered when a faculty member submits a leave application for admin approval.",
                    InAppEnabled = true,
                    EmailEnabled = true,
                    IconClass = "fa-solid fa-calendar-xmark",
                    ColorClass = "warning"
                },
                new NotificationEventSetting
                {
                    EventCode = "STUDENT_LEAVE_APPLIED",
                    EventName = "Student Leave Request",
                    Category = "Academics",
                    Description = "Triggered when a student/guardian submits an absence or leave request.",
                    InAppEnabled = true,
                    EmailEnabled = false,
                    IconClass = "fa-solid fa-user-clock",
                    ColorClass = "info"
                },
                new NotificationEventSetting
                {
                    EventCode = "MOCK_TEST_PUBLISHED",
                    EventName = "Mock Test / Exam Published",
                    Category = "Academics",
                    Description = "Triggered when a new test schedule, mock test, or term exam is announced.",
                    InAppEnabled = true,
                    EmailEnabled = true,
                    IconClass = "fa-solid fa-file-circle-check",
                    ColorClass = "purple"
                },
                new NotificationEventSetting
                {
                    EventCode = "ANNOUNCEMENT_PUBLISHED",
                    EventName = "Notice / Circular Broadcast",
                    Category = "Communications",
                    Description = "Triggered when an official announcement or circular is published by the institute.",
                    InAppEnabled = true,
                    EmailEnabled = true,
                    IconClass = "fa-solid fa-bullhorn",
                    ColorClass = "amber"
                },
                new NotificationEventSetting
                {
                    EventCode = "TC_GENERATED",
                    EventName = "Transfer Certificate (TC) Issued",
                    Category = "Admissions",
                    Description = "Triggered when a student Transfer Certificate is approved and generated.",
                    InAppEnabled = true,
                    EmailEnabled = false,
                    IconClass = "fa-solid fa-file-shield",
                    ColorClass = "rose"
                },
                new NotificationEventSetting
                {
                    EventCode = "LOW_ATTENDANCE",
                    EventName = "Low Attendance Warning",
                    Category = "Academics",
                    Description = "Automated alert when a student's attendance drops below minimum threshold.",
                    InAppEnabled = true,
                    EmailEnabled = true,
                    IconClass = "fa-solid fa-triangle-exclamation",
                    ColorClass = "danger"
                }
            };
        }
    }
}
