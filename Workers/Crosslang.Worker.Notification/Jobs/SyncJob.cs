using System;
using Crosslang.Worker.Notification.Hubs;
using Crosslang.Worker.Notification.Services;
using CrossLang.Models;
using CrossLang.QueueHelper;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Crosslang.Worker.Notification.Jobs
{

    public class SyncJob
    {

        private IConfiguration _configuration;
        private ILogger<SyncJob> _logger;
        private INotificationService _notiService;

        public SyncJob(IConfiguration config, ILogger<SyncJob> logger, INotificationService notiService)
        {
            _logger = logger;
            _configuration = config;
            _notiService = notiService;
        }

        public void Start()
        {
            RabbitMQHelper.Subscribe<NotificationMessage>(_configuration["RabbitMQ:QueueName"], (message) =>
            {
                Dequeue(message);
            });
        }

        public void Dequeue(RabbitMQMessage<NotificationMessage> message)
        {
            try
            {
                _logger.LogInformation("Dequeue: {message}", JsonConvert.SerializeObject(message));

                if (message.Body != null)
                {
                    //save to DB

                    _notiService.SendToUserID(message.Body, message.Body.UserIDs.First());
                }

            }
            catch (Exception e)
            {
                _logger.LogError("Error-Dequeue: {e}", e.ToString());
            }
        }
    }
}

