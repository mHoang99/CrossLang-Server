using System;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Interfaces.IRepository;
using CrossLang.ApplicationCore.Interfaces.IService;
using CrossLang.Library;
using CrossLang.Models;
using Microsoft.AspNetCore.Http;

namespace CrossLang.ApplicationCore.Services
{
    public class LessonCommentService : BaseService<LessonComment>, ILessonCommentService
    {
        public LessonCommentService(ILessonCommentRepository repository, IHttpContextAccessor httpContextAccessor, SessionData sessionData) : base(repository, httpContextAccessor, sessionData)
        {
        }

        protected override void BeforeAdd(ref LessonComment entity)
        {
            base.BeforeAdd(ref entity);
            entity.UserID = _sessionData.ID;
        }

        protected override void AsyncAfterAdd(LessonComment entity, HttpContext httpContext)
        {
            base.AsyncAfterAdd(entity, httpContext);
        }

        public override ServiceResult QueryList(LessonComment entity, List<FilterObject> filters, string formula, string sortBy = "ModifiedDate", string sortDirection = "desc", int pageNum = 1, int pageSize = 10)
        {
            List<dynamic> list = this._repository.QueryListByView("view_lesson_comment", entity, filters, formula, sortBy, sortDirection, pageNum, pageSize);

            long dbCount = this._repository.QueryListCount(entity, filters, formula);

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

