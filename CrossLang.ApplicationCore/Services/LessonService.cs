using System;
using System.Data;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Interfaces.IRepository;
using CrossLang.ApplicationCore.Interfaces.IService;
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

