using System;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Interfaces.IService;
using CrossLang.Library;

namespace CrossLang.API.Controllers
{
    public class LessonController : BaseApiController<Lesson>
    {
        public LessonController(ILessonService service, SessionData sessionData) : base(service, sessionData)
        {
        }
    }
}

