using System;
using System.Threading.Tasks;
using IMS.Models.Notification;

namespace IMS.DAL.Interfaces
{
    public interface INotificationConfigDAL
    {
        Task<NotificationConfigModel> GetConfigAsync(Guid tenantId);
        Task<bool> SaveConfigAsync(Guid tenantId, NotificationConfigModel config);
    }
}
