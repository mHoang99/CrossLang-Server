using System;
using CrossLang.ApplicationCore.Entities;

namespace CrossLang.ApplicationCore.Interfaces.IService
{
    public interface INotificationService : IBaseService<Notification>
    {
        public void SendNotification(Notification notification);
    }
}

