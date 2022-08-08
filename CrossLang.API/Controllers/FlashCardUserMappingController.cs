using System;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.Library;

namespace CrossLang.API.Controllers
{
    public class FlashCardUserMappingController : BaseApiController<FlashCardUserMapping>
    {
        public FlashCardUserMappingController(IBaseService<FlashCardUserMapping> service, SessionData sessionData) : base(service, sessionData)
        {
        }
    }
}

