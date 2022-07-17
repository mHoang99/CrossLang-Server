using System;
using System.Data;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Interfaces.IRepository;
using CrossLang.ApplicationCore.Interfaces.IService;
using CrossLang.DBHelper;
using CrossLang.Library;
using CrossLang.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace CrossLang.ApplicationCore.Services
{
    public class LessonService : BaseService<Lesson>, ILessonService
    {
        private IDictionaryWordRepository _dictionaryWordRepository;

        private IFlashCardCollectionRepository _flashCardCollectionRepository;

        private IFlashCardRepository _flashCardRepository;

        private IBaseRepository<UserLessonStatus> _userLessonStatusRepository;

        private INotificationService _notiService;

        private IDbConnection _connection;

        public LessonService(ILessonRepository repository,
            IDictionaryWordRepository dictionaryWordRepository,
            IFlashCardCollectionRepository flashCardCollectionRepository,
            IHttpContextAccessor httpContextAccessor, SessionData sessionData,
            IBaseRepository<UserLessonStatus> userLessonStatusRepo,
            IDBContext dbContext, IFlashCardRepository flashCardRepository, INotificationService notificationService) : base(repository, httpContextAccessor, sessionData)
        {
            _dictionaryWordRepository = dictionaryWordRepository;
            _flashCardCollectionRepository = flashCardCollectionRepository;
            _flashCardRepository = flashCardRepository;
            _userLessonStatusRepository = userLessonStatusRepo;
            _connection = dbContext.GetConnection();
            _notiService = notificationService;
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
            }
            catch (Exception e)
            {

            }
        }

        protected override void AfterUpdate(Lesson entity, Lesson oldEntity)
        {
            base.AfterUpdate(entity, oldEntity);

            LessonContentMongo mongoLessonContent = new LessonContentMongo
            {
                LessonID = entity.ID,
                LessonContent = entity.Content
            };


            ((ILessonRepository)_repository).UpdateLessonContentMongo(entity.ID, mongoLessonContent);
        }


        protected override void AsyncAfterUpdate(Lesson entity, Lesson oldEntity)
        {
            base.AsyncAfterUpdate(entity, oldEntity);

            var fcc = new FlashCardCollection
            {
                LessonID = entity.ID
            };


            var flashCardCollection = _flashCardCollectionRepository.GetEntityByColumns(fcc, new List<string> { nameof(FlashCardCollection.LessonID) });

            if (flashCardCollection != null)
            {
                var tempfc = new FlashCard
                {
                    FlashCardCollectionID = flashCardCollection.ID
                };

                var flashCards = _flashCardRepository.GetEntitiesByColumns(tempfc, new List<string> { nameof(FlashCard.FlashCardCollectionID) });

                var newDictionaryWordIds = entity.DictionaryWordIDs?.FindAll(x =>
                {
                    return !flashCards.Exists(y => x == y.ID);
                });

                var removedflashCardIds = flashCards?.FindAll(x =>
                {
                    return !entity.DictionaryWordIDs?.Exists(y => y == x.ID) ?? false;
                }).Select(x => x.ID).ToList();

                //Thêm mới
                AddNewFlashCards(newDictionaryWordIds, flashCardCollection.ID);

                //Xoá
                RemoveFlashCards(removedflashCardIds, flashCardCollection.ID);
            }
        }


        private void AddNewFlashCards(List<long> wordsIDs, long collectionID)
        {
            var newFlashCards = wordsIDs.Select(x => new FlashCard
            {
                UserID = _sessionData.ID,
                CreatedBy = _sessionData.Username,
                ModifiedBy = _sessionData.Username,
                DictionaryWordID = x,
                FlashCardCollectionID = collectionID,
                EntityState = Enums.EntityState.ADD
            });

            foreach (var fc in newFlashCards)
            {
                var res = _flashCardRepository.Add(fc);
            }
        }

        private void RemoveFlashCards(List<long> flashCardIds, long collectionID)
        {
            _flashCardRepository.DeleteByIDs(flashCardIds);

            _flashCardCollectionRepository.RemoveUserProgressOfCollectionByFlashCardIDs(collectionID, flashCardIds);
        }


        protected override void AsyncAfterDelete(Lesson oldEntity)
        {
            base.AsyncAfterDelete(oldEntity);
            ((ILessonRepository)_repository).DeleteLessonContentMongo(oldEntity.ID);
        }

        public override ServiceResult GetDetailsById(long id)
        {
            var entity = _repository.GetDetailsById(id).FirstOrDefault();

            if (entity != null)
            {
                var relatedWords = ((ILessonRepository)_repository).GetRelatedWords(id);

                entity.AddOrUpdate("Content", ((ILessonRepository)_repository).GetLessonContentMongo(id)?.FirstOrDefault()?.LessonContent);
                entity.AddOrUpdate("DictionaryWords", relatedWords.Select(x => x.Text).ToList());
                entity.AddOrUpdate("DictionaryWordIDs", relatedWords.Select(x => x.ID).ToList());

                if (relatedWords.Count() > 0)
                {
                    entity.AddOrUpdate("FlashCardCollectionID", relatedWords[0].FlashCardCollectionID);
                }

                if (entity.TryGetValue("ID", out var lessonId))
                {
                    CreateTask(() =>
                    {
                        var ulsEntity = new UserLessonStatus
                        {
                            IsFinished = false,
                            UserID = _sessionData.ID,
                            LessonID = long.Parse(lessonId.ToString()),
                            CreatedBy = _sessionData.Username,
                            CreatedDate = DateTime.Now,
                            ModifiedBy = _sessionData.Username,
                            ModifiedDate = DateTime.Now
                        };

                        var oldUlsEntity = _userLessonStatusRepository.GetEntityByColumns(ulsEntity, new List<string>
                        {
                            "UserID", "LessonID"
                        });

                        if (oldUlsEntity == null)
                        {
                            _userLessonStatusRepository.Add(ulsEntity);
                        }
                    });
                }
            }


            serviceResult.SuccessState = true;

            serviceResult.Data = entity;

            return serviceResult;
        }

        public ServiceResult GetAttempHistory(long lessonID)
        {
            var listAttemp = ((ILessonRepository)_repository).GetAttempHistory(lessonID);
            serviceResult.SuccessState = true;
            serviceResult.Data = listAttemp;

            //var notification = new Notification { Message = JsonConvert.SerializeObject(listAttemp) };

            //_notiService.SendNotification(notification);

            return serviceResult;
        }

        public ServiceResult FinishLesson(Lesson entity)
        {
            var tempEntity = new UserLessonStatus
            {
                IsFinished = true,
                UserID = _sessionData.ID,
                LessonID = entity.ID,
                CreatedBy = _sessionData.Username,
                CreatedDate = DateTime.Now,
                ModifiedBy = _sessionData.Username,
                ModifiedDate = DateTime.Now
            };

            var existedEntity = _userLessonStatusRepository.GetEntityByColumns(tempEntity, new List<string>
            {
                "UserID", "LessonID"
            });

            if (existedEntity == null)
            {
                this._userLessonStatusRepository.Add(tempEntity);
            }
            else
            {
                tempEntity.ID = existedEntity.ID;
                this._userLessonStatusRepository.UpdateFields(new List<string> { "IsFinished", "ModifiedDate", "ModifiedBy" }, tempEntity);
            }

            serviceResult.SuccessState = true;

            return serviceResult;
        }

        public ServiceResult GetLearningLessonList()
        {
            var lessons = ((ILessonRepository)_repository).GetLearningLessonList();

            serviceResult.SuccessState = true;
            serviceResult.Data = lessons;

            return serviceResult;
        }

        public ServiceResult GetLessonList(Lesson entity, List<FilterObject> filters, string formula, string sortBy, string sortDirection, int pageNum, int pageSize)
        {
            var lessons = ((ILessonRepository)_repository).GetLessonList(entity, filters, formula, sortBy, sortDirection, pageNum, pageSize);

            serviceResult.SuccessState = true;
            serviceResult.Data = lessons;

            return serviceResult;
        }
    }
}

