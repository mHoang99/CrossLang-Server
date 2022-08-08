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

        protected override bool CustomValidate(FlashCard entity, List<string>? fields)
        {
            if(entity.EntityState == Enums.EntityState.ADD)
            {
               var existedEntity = _repository.GetEntityByColumns(entity,new List<string> { nameof(FlashCard.FlashCardCollectionID), nameof(FlashCard.DictionaryWordID)});

                if(existedEntity != null)
                {
                    serviceResult.SuccessState = false;
                    serviceResult.UserMsg = string.Format(Properties.Resources.CrossLang_ResponseMessage_FlashCardExisted);
                    serviceResult.DevMsg = string.Format(Properties.Resources.CrossLang_ResponseMessage_FlashCardExisted);
                    return false;
                }
            }

            return base.CustomValidate(entity, fields);
        }

        protected override bool ValidateBeforeDelete(FlashCard oldEntity)
        {
            return oldEntity.UserID == _sessionData.ID;
        }

        protected override void AfterDelete(FlashCard oldEntity)
        {
            base.AfterDelete(oldEntity);
        }

        protected override void BeforeMassAdd(ref List<FlashCard> entities)
        {
            base.BeforeMassAdd(ref entities);

            if (entities.Count > 0)
            {
                if (entities[0].FlashCardCollectionID != null)
                {
                    var cols = new List<string> { "FlashCardCollectionID" };
                    var fcs = ((IFlashCardRepository)_repository).GetEntitiesByColumns(entities[0], cols);

                    foreach (var fc in fcs)
                    {
                        var fcindex = entities.FindIndex(x => x.DictionaryWordID == fc.DictionaryWordID);
                        if (fcindex > -1)
                        {
                            entities.RemoveAt(fcindex);
                        }
                    }
                }
            }
        }

        protected override void AsyncAfterMassAdd(List<FlashCard> entities)
        {
            base.AsyncAfterMassAdd(entities);

            if(entities.Count > 0)
            {
                flashCardCollectionRepository.UpdateIndividualsProgress(entities[0].FlashCardCollectionID ?? 0);
            }
        }
    }
}

