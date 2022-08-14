using System;
using System.Data;
using Crosslang.Worker.Notification.Hubs;
using CrossLang.DBHelper;
using Microsoft.AspNetCore.SignalR;

namespace Crosslang.Worker.Notification.Services
{
    public class NotificationService : INotificationService
    {
        private IDBContext _dbContext;

        private IDbConnection _connection;

        private IConfiguration _configuration;

        private readonly ILogger<NotificationService> _logger;

        private IHubContext<NotificationHub> _notiHubCtx;

        public NotificationService(IDBContext dbContext, IConfiguration configuration, ILogger<NotificationService> logger, IHubContext<NotificationHub> notiHubCtx)
        {
            _dbContext = dbContext;
            _connection = dbContext.GetConnection();
            _configuration = configuration;
            _logger = logger;
            _notiHubCtx = notiHubCtx;
        }

        public async Task SendToUserID(object obj, long userID)
        {
            await _notiHubCtx.Clients.All.SendAsync("ReceiveMessage", obj);
        }
    }
}

