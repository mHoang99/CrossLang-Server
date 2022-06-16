using System;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Interfaces.IRepository;
using CrossLang.ApplicationCore.Interfaces.IService;
using CrossLang.Library;
using Microsoft.AspNetCore.Http;

namespace CrossLang.ApplicationCore.Services
{
    public class FlashCardService : BaseService<FlashCard>, IFlashCardService
    {
        public FlashCardService(IFlashCardRepository repository, IHttpContextAccessor httpContextAccessor, SessionData sessionData) : base(repository, httpContextAccessor, sessionData)
        {
        }

        protected override void BeforeAdd(ref FlashCard entity)
        {
            base.BeforeAdd(ref entity);

            entity.UserID = _sessionData.ID;
        }

        protected override bool ValidateBeforeDelete(FlashCard oldEntity)
        {
            return oldEntity.UserID == _sessionData.ID;
        }
    }
}

