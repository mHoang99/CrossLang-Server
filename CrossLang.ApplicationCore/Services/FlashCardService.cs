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
        IFlashCardCollectionRepository flashCardCollectionRepository;

        public FlashCardService(IFlashCardRepository repository, IFlashCardCollectionRepository fccRepository, IHttpContextAccessor httpContextAccessor, SessionData sessionData) : base(repository, httpContextAccessor, sessionData)
        {
            flashCardCollectionRepository = fccRepository;
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

        protected override void AfterDelete(FlashCard oldEntity)
        {
            base.AfterDelete(oldEntity);
        }
    }
}

