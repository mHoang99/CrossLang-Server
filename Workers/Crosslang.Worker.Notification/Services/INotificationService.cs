using System;
namespace Crosslang.Worker.Notification.Services
{
    public interface INotificationService
    {
        public Task SendToUserID(object obj, long userID);
    }
}

