using System;
using CrossLang.API.Models.Requests;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Interfaces.IService;
using CrossLang.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrossLang.API.Controllers
{
    public class FlashCardController : BaseApiController<FlashCard>
    {
        public FlashCardController(IFlashCardService service, SessionData sessionData) : base(service, sessionData)
        {
            Service = service;
        }

        public IFlashCardService Service { get; }
    }
}

