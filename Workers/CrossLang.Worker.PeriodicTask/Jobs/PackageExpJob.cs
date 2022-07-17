using System;
using System.Data;
using System.Linq;
using CrossLang.DBHelper;
using CrossLang.Models;
using CrossLang.QueueHelper;
using CrossLang.Worker.PeriodicTask.Entities;
using Dapper;

namespace CrossLang.Worker.PeriodicTask.Jobs
{
    public class PackageExpJob : IJob
    {
        private IDBContext _dbContext;

        private IDbConnection _connection;

        private IConfiguration _configuration;

        private readonly ILogger<PackageExpJob> _logger;


        public PackageExpJob(ILogger<PackageExpJob> logger, IDBContext dBContext, IConfiguration configuration)
        {
            _logger = logger;
            _dbContext = dBContext;
            _connection = dBContext.GetConnection();
            _configuration = configuration;
        }

        public void Start()
        {
            try
            {
                var expUsers = getExpUser();

                if (expUsers.Count() > 0)
                {
                    setUserPackageToFree(expUsers.Select(x => x.ID ?? 0).ToList());

                    foreach (var user in expUsers)
                    {
                        sendEmailToUser(user);
                    }
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
            }
        }

        public List<User> getExpUser()
        {
            var query = "SELECT * FROM user WHERE expDate < NOW() AND expDate is not null";

            return _connection.Query<User>(query).ToList();
        }

        public void setUserPackageToFree(List<long> ids)
        {
            var idsString = string.Join(",", ids);

            var query = $"UPDATE user SET Package = 1, expDate = null WHERE ID IN ({idsString})";

            _connection.ExecuteAsync(query);
        }

        public void sendEmailToUser(User user)
        {
            if(user.Email == null)
            {
                return;
            }

            var emailQueue = _configuration["RabbitMQ:EmailQueue"];

            RabbitMQMessage<EmailMessage> message = new RabbitMQMessage<EmailMessage>
            {
                Body = new EmailMessage
                {
                    To = user.Email,
                    Subject = "[CrossLang] Thông báo tài khoản hết hạn",
                    Body = $"<div><div>Xin chào {user.FullName},</div>Chúng tôi xin trân trọng thông báo tài khoản nâng cấp của bạn đã hết thời hạn sử dụng, và sẽ được trở lại thành tài khoản <b>FREE</b>. Để có thể tiếp tục sử dụng các tính năng nâng cao, vui lòng thực hiện nâng cấp tài khoản.<div>Thân trọng.</div><div></div></div>"
                },
                UserID = user.ID
            };

            RabbitMQHelper.Enqueue(emailQueue, message);
        }
    }
}

