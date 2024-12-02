using RevOnRental.Domain.Enums;
using RevOnRental.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Application.Interfaces
{
    public interface INotificationService
    {
        Task CreateNotification(int senderId, int receiverId, string message, NotificationType type);
        Task<IEnumerable<Notification>> GetUserNotifications(int userId);
    }

}
