using System;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Interfaces.IService;
using CrossLang.Library;
using CrossLang.QueueHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CrossLang.ApplicationCore.Services
{
    public class NotificationService : BaseService<Notification>, INotificationService
    {
        private IConfiguration _configuration;

        public NotificationService(IBaseRepository<Notification> repository, IHttpContextAccessor httpContextAccessor, SessionData sessionData, IConfiguration configuration) : base(repository, httpContextAccessor, sessionData)
        {
            _configuration = configuration;  
        }

        public void SendNotification(Notification notification)
        {
            var notiQueue = _configuration["RabbitMQ:NotificationQueue"];

            RabbitMQMessage<Notification> message = new RabbitMQMessage<Notification> { Body = notification, UserID = _sessionData.ID };

            RabbitMQHelper.Enqueue(notiQueue, message);
        }
    }
}

