using System;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.Library;

namespace CrossLang.API.Controllers
{
    public class LessonCategoryController : BaseApiController<LessonCategory>
    {
        public LessonCategoryController(IBaseService<LessonCategory> service, SessionData sessionData) : base(service, sessionData)
        {
        }
    }
}

