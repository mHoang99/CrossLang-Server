﻿using System;
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
        private IBaseService<FlashCardCollectionUserMapping> _flashCardCollectionUserMappingRepository;
        private IBaseService<FlashCardCollectionUserMapping> _flashCardCollectionUserMappingService;

        public FlashCardCollectionService(IFlashCardCollectionRepository repository,
            IBaseService<FlashCardCollectionUserMapping> fccumRepository,
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

        public ServiceResult GetListCollection(FlashCardCollection entity, List<FilterObject> filters, string formula , int pageNum, int pageSize)
        {
            var (list, dbCount) = this._repository.QueryListByView("view_flash_card_collection", entity, filters, formula, "ModifiedDate", "desc", pageNum, pageSize);

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

        public ServiceResult GetListCollectionWithProgress(int pageNum, int pageSize, bool type)
        {
            var (list, dbCount) = ((IFlashCardCollectionRepository)_repository).QueryListWithProgress(_sessionData.ID, pageNum, pageSize, type);

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

