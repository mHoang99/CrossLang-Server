using System;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.Library;

namespace CrossLang.API.Controllers
{
    public class FlashCardCollectionProgressUserMappingController : BaseApiController<FlashCardCollectionProgressUserMapping>
    {
        public FlashCardCollectionProgressUserMappingController(IBaseService<FlashCardCollectionProgressUserMapping> service, SessionData sessionData) : base(service, sessionData)
        {
        }
    }
}

