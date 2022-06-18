using System;
using System.Data;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Interfaces.IRepository;
using CrossLang.ApplicationCore.Interfaces.IService;
using CrossLang.Library;
using CrossLang.Models;
using Microsoft.AspNetCore.Http;

namespace CrossLang.ApplicationCore.Services
{
    public class LessonService : BaseService<Lesson>, ILessonService
    {
        private IDictionaryWordRepository _dictionaryWordRepository;

        private IFlashCardCollectionRepository _flashCardCollectionRepository;

        private IFlashCardRepository _flashCardRepository;

        private IDbConnection _connection;

        public LessonService(ILessonRepository repository,
            IDictionaryWordRepository dictionaryWordRepository,
            IFlashCardCollectionRepository flashCardCollectionRepository,
            IHttpContextAccessor httpContextAccessor, SessionData sessionData,
            IDBContext dbContext, IFlashCardRepository flashCardRepository) : base(repository, httpContextAccessor, sessionData)
        {
            _dictionaryWordRepository = dictionaryWordRepository;
            _flashCardCollectionRepository = flashCardCollectionRepository;
            _flashCardRepository = flashCardRepository;
            _connection = dbContext.GetConnection();
        }

        protected override void AfterAdd(ref Lesson entity)
        {
            base.AfterAdd(ref entity);

            LessonContentMongo mongoLessonContent = new LessonContentMongo
            {
                LessonID = entity.ID,
                LessonContent = entity.Content
            };

            ((ILessonRepository)_repository).InsertLessonContentMongo(mongoLessonContent);
        }

        protected override void AsyncAfterAdd(Lesson entity, HttpContext httpContext)
        {
            base.AsyncAfterAdd(entity, httpContext);

            try
            {
                var flashCardCollection = new FlashCardCollection
                {
                    UserID = _sessionData.ID,
                    Title = entity.Title,
                    Description = entity.Description,
                    IsPublic = true,
                    IsCertified = true,
                    LessonID = entity.ID,
                    CreatedBy = _sessionData.Username,
                    ModifiedBy = _sessionData.Username,
                    EntityState = Enums.EntityState.ADD
                };


                long newFCCID = this._flashCardCollectionRepository.Add(flashCardCollection);



                if (newFCCID > 0 && entity.DictionaryWordIDs != null)
                {
                    var flashCards = entity.DictionaryWordIDs.Select(x => new FlashCard
                    {
                        UserID = _sessionData.ID,
                        CreatedBy = _sessionData.Username,
                        ModifiedBy = _sessionData.Username,
                        DictionaryWordID = x,
                        FlashCardCollectionID = newFCCID,
                        EntityState = Enums.EntityState.ADD
                    });

                    foreach (var fc in flashCards)
                    {
                        var res = _flashCardRepository.Add(fc);
                    }
                }
            } catch (Exception e)
            {
                
            }

           
        }

        protected override void AsyncAfterDelete(Lesson oldEntity)
        {
            base.AsyncAfterDelete(oldEntity);
            ((ILessonRepository)_repository).DeleteLessonContentMongo(oldEntity.ID);
        }

        public override ServiceResult GetDetailsById(long id)
        {
            var entity = _repository.GetEntityById(id);

            var relatedWords = ((ILessonRepository)_repository).GetRelatedWords(id);

            entity.Content = ((ILessonRepository)_repository).GetLessonContentMongo(id)?.FirstOrDefault()?.LessonContent;

            entity.DictionaryWords = relatedWords.Select(x => x.Text).ToList();
            entity.DictionaryWordIDs = relatedWords.Select(x => x.ID).ToList();

            serviceResult.SuccessState = true;

            serviceResult.Data = entity;

            return serviceResult;
        }
    }
}

