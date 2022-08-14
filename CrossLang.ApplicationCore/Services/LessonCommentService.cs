using System;
using System.Linq;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Interfaces.IRepository;
using CrossLang.ApplicationCore.Interfaces.IService;
using CrossLang.Library;
using CrossLang.Models;
using CrossLang.QueueHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CrossLang.ApplicationCore.Services
{
    public class LessonCommentService : BaseService<LessonComment>, ILessonCommentService
    {
        private IConfiguration _configuration;

        public LessonCommentService(ILessonCommentRepository repository, IHttpContextAccessor httpContextAccessor, SessionData sessionData, IConfiguration configuration) : base(repository, httpContextAccessor, sessionData)
        {
            _configuration = configuration;
        }

        protected override void BeforeAdd(ref LessonComment entity)
        {
            base.BeforeAdd(ref entity);
            entity.UserID = _sessionData.ID;
        }

        protected override void AsyncAfterAdd(LessonComment entity, HttpContext httpContext)
        {
            base.AsyncAfterAdd(entity, httpContext);

            if(entity.ReplyToID != null)
            {
                var parent = _repository.GetEntityById(entity.ReplyToID ?? 0);

                var children = _repository.GetEntitiesByColumns(entity, new List<string> { "ReplyToID" });

                var users = new List<long>();
                users.Add(parent.UserID ?? 0);
                users.AddRange(children.Where(x => x.UserID.HasValue && x.UserID != entity.UserID).Select(x => x.UserID ?? 0));
               
                entity.ReplyToUserID = parent.UserID;
                entity.ReplyToUserIDs = users;

                SendNotiToUser(entity);

            }
        }

        private void SendNotiToUser(LessonComment entity)
        {
            var emailQueue = _configuration["RabbitMQ:NotificationQueue"];

            RabbitMQMessage<NotificationMessage> message = new RabbitMQMessage<NotificationMessage>
            {
                Body = new NotificationMessage
                {
                    SenderUserID = entity.UserID ?? 0,
                    HtmlTemplate = "<div></div>",
                    SaveToDB = true,
                    RelatedEndpoint = $"/education/lesson/preview/{entity.LessonID}",
                    Type = NotificationType.SEND_TO_ONE,
                    UserIDs = entity.ReplyToUserIDs
                },
                UserID = _sessionData.ID
            };

            RabbitMQHelper.Enqueue(emailQueue, message);
        }

        public override ServiceResult QueryList(LessonComment entity, List<FilterObject> filters, string formula, string sortBy = "ModifiedDate", string sortDirection = "desc", int pageNum = 1, int pageSize = 10)
        {
            var (list, dbCount) = this._repository.QueryListByView("view_lesson_comment", entity, filters, formula, sortBy, sortDirection, pageNum, pageSize);

            serviceResult.SuccessState = true;
            serviceResult.Data = new
            {
                Data = list,
                Summary = new
                {
                    Count = dbCount
                }
            };

            return serviceResult;
        }
    }
}

