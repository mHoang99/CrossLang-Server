using System;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrossLang.API.Controllers
{
    public class FlashCardCollectionUserMappingController : BaseApiController<FlashCardCollectionUserMapping>
    {
        public FlashCardCollectionUserMappingController(IBaseService<FlashCardCollectionUserMapping> service, SessionData sessionData) : base(service, sessionData)
        {
        }
    }
}

