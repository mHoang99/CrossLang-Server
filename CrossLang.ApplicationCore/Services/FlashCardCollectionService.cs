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
    public class FlashCardCollectionService : BaseService<FlashCardCollection>, IFlashCardCollectionService
    {
        private IBaseRepository<FlashCardCollectionUserMapping> _flashCardCollectionUserMappingRepository;
        private IBaseService<FlashCardCollectionUserMapping> _flashCardCollectionUserMappingService;

        public FlashCardCollectionService(IFlashCardCollectionRepository repository,
            IBaseRepository<FlashCardCollectionUserMapping> fccumRepository,
            IBaseService<FlashCardCollectionUserMapping> fccumService,
            IHttpContextAccessor httpContextAccessor, SessionData sessionData) : base(repository, httpContextAccessor, sessionData)
        {
            _flashCardCollectionUserMappingRepository = fccumRepository;
            _flashCardCollectionUserMappingService = fccumService;
        }

        protected override void BeforeAdd(ref FlashCardCollection entity)
        {
            base.BeforeAdd(ref entity);
            if (entity.EntityState == Enums.EntityState.ADD)
            {
                entity.UserID = this._sessionData.ID;
            }

        }

        public ServiceResult GetListCollection(FlashCardCollection entity, List<FilterObject> filters, int pageNum, int pageSize)
        {
            var list = this._repository.QueryListByView("view_flash_card_collection", entity, filters, pageNum, pageSize);

            long dbCount = this._repository.QueryListByViewCount("view_flash_card_collection", entity, filters);

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

        protected override void AfterAdd(ref FlashCardCollection entity)
        {
            base.AfterAdd(ref entity);

            if (entity.EntityState == Enums.EntityState.ADD)
            {
                FlashCardCollectionUserMapping fccum = new FlashCardCollectionUserMapping
                {
                    UserID = entity.UserID,
                    IsFollowing = true,
                    FlashCardCollectionID = entity.ID,
                    Rating = 0
                };
                _flashCardCollectionUserMappingService.Add(fccum);
            }
        }


        public override ServiceResult GetDetailsById(long id)
        {
            serviceResult.Data = ((IFlashCardCollectionRepository)_repository).GetCollectionDetails(id);
            serviceResult.SuccessState = true;

            return serviceResult;
        }

    }
}

