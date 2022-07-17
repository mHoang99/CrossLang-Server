using System;
using CrossLang.Models;
using CrossLang.QueueHelper;
using CrossLang.Worker.Email.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CrossLang.Worker.Email.Jobs
{
    public class SyncJob
    {
        private IConfiguration _configuration;
        private ILogger<SyncJob> _logger;
        private IEmailService _emailService;

        public SyncJob(IConfiguration config, ILogger<SyncJob> logger, IEmailService emailService)
        {
            _logger = logger;
            _configuration = config;
            _emailService = emailService;
        }

        public void Start()
        {
            RabbitMQHelper.Subscribe<EmailMessage>(_configuration["RabbitMQ:QueueName"], (message) =>
            {
                Dequeue<EmailMessage>(message);
            });
        }

        public void Dequeue<T>(RabbitMQMessage<EmailMessage> message)
        {
            try
            {
                _logger.LogInformation("Dequeue: {message}", JsonConvert.SerializeObject(message));

                if(message.Body != null)
                {
                    _emailService.SendEmail(message.Body);
                }

            } catch (Exception e)
            {
                _logger.LogError("Error-Dequeue: {e}", e.ToString());
            }
        }
    }
}

