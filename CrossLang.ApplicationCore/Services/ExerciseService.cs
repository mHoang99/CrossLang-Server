using System;
using System.ComponentModel;
using System.Linq;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Enums;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Interfaces.IRepository;
using CrossLang.ApplicationCore.Interfaces.IService;
using CrossLang.Library;
using CrossLang.Models;
using Microsoft.AspNetCore.Http;

namespace CrossLang.ApplicationCore.Services
{
    public class ExerciseService : BaseService<Exercise>, IExerciseService
    {
        ILessonRepository _lessonRepository;

        public ExerciseService(IExerciseRepository repository, IHttpContextAccessor httpContextAccessor, SessionData sessionData, ILessonRepository lessonRepository) : base(repository, httpContextAccessor, sessionData)
        {
            _lessonRepository = lessonRepository;
        }

        protected override bool CustomValidate(Exercise entity, List<string>? fields)
        {
            return base.CustomValidate(entity, fields);
        }

        protected override void AfterAdd(ref Exercise entity)
        {
            base.AfterAdd(ref entity);

            if (entity.Questions == null)
            {
                return;
            }

            foreach (var question in entity.Questions)
            {
                question.ExerciseID = entity.ID;
            }

            ((IExerciseRepository)_repository).InsertQuestionsMongo(entity.Questions);
        }


        protected override void AfterUpdate(Exercise entity, Exercise oldEntity)
        {
            base.AfterUpdate(entity, oldEntity);

            if (entity.Questions == null)
            {
                return;
            }

            foreach (var question in entity.Questions)
            {
                question.ExerciseID = entity.ID;
            }


            ((IExerciseRepository)_repository).DeleteExerciseQuestionsMongo(entity.ID);
            ((IExerciseRepository)_repository).InsertQuestionsMongo(entity.Questions);
        }



        protected override void AfterDelete(Exercise oldEntity)
        {
            base.AfterDelete(oldEntity);

            ((IExerciseRepository)_repository).DeleteExerciseQuestionsMongo(oldEntity.ID);
        }

        public override ServiceResult GetDetailsById(long id)
        {
            var exercise = _repository.GetEntityById(id);

            if (exercise != null)
            {
                if (!CheckPackagePermission(exercise.Package) && !_sessionData.IsEmployee)
                {
                    serviceResult.SuccessState = false;

                    serviceResult.Code = 403;

                    return serviceResult;
                }

                exercise.Questions = ((IExerciseRepository)_repository).GetExerciseQuestionsMongo(id);

                if (_sessionData.RoleID == (int)RoleEnum.USER)
                {
                    foreach (var question in exercise.Questions)
                    {
                        question.Options.Shuffle();
                        question.Answers = null;
                    }
                }
            }

            serviceResult.SuccessState = true;
            serviceResult.Data = exercise;

            return serviceResult;
        }

        public ServiceResult Submit(ExerciseAttempMongo entity)
        {

            if (entity.ExerciseID is null)
            {
                return serviceResult;
            }

            entity.UserID = _sessionData.ID;

            var questions = ((IExerciseRepository)_repository).GetExerciseQuestionsMongo(entity.ExerciseID ?? 0);

            if (questions != null)
            {
                ScoreAttemp(questions, ref entity);

                ((IExerciseRepository)_repository).InsertExerciseAttempMongo(entity);
                serviceResult.SuccessState = true;
                serviceResult.Data = entity;
            }

            return serviceResult;
        }

        private void ScoreAttemp(List<QuestionMongo> questions, ref ExerciseAttempMongo entity)
        {
            var correctAnswers = 0;
            foreach (var question in questions)
            {
                if (entity.AttempAnswers == null)
                {
                    break;
                }

                var attempAnswer = entity.AttempAnswers.Find(x => x.QuestionID.Equals(question.ID));
                if (attempAnswer?.Answers.Count() != question.Answers.Count())
                {
                    break;
                }

                foreach (var answer in question.Answers)
                {
                    if (!attempAnswer.Answers.Exists(x => x.Equals(answer)))
                    {
                        correctAnswers = correctAnswers - 1;
                        break;
                    };
                }

                correctAnswers = correctAnswers + 1;
            }

            entity.CorrectAnswerCount = correctAnswers;
            entity.TotalAnswerCount = questions.Count();
        }

        public ServiceResult GetAttempHistory(long exerciseId)
        {
            var listAttemp = ((IExerciseRepository)_repository).GetAttempHistory(exerciseId);
            serviceResult.SuccessState = true;
            serviceResult.Data = listAttemp;

            return serviceResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public virtual ServiceResult QueryList(Exercise entity, List<FilterObject> filters, string formula, string sortBy = "ModifiedDate", string sortDirection = "desc", int pageNum = 1, int pageSize = 10)
        {
            var (list, dbCount) = this._repository.QueryList(entity, filters, formula, sortBy, sortDirection, pageNum, pageSize);

            if (list.Count > 0)
            {
                var questions = ((IExerciseRepository)_repository).GetExercisesQuestionsMongo(list.Select(x => x.ID).ToList());
                foreach (var item in list)
                {
                    item.Questions = questions.FindAll(x => { return x.ExerciseID == item.ID; });
                }
            }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public virtual ServiceResult QueryExerciseAttempList(ExerciseAttempMongo entity, List<FilterObject> filters, string formula, string sortBy = "ModifiedDate", string sortDirection = "desc", int pageNum = 1, int pageSize = 10)
        {
            var (list, count) = ((IExerciseRepository)_repository).QueryExerciseAttempList(entity, filters);

            var distinctList = list.GroupBy(x => x.UserID).Select(x => x.First()).ToList();

            if (sortDirection == "desc")
            {
                distinctList = distinctList.OrderByDescending(x => x.GetType().GetProperty(sortBy)?.GetValue(x)).ToList();
            }

            if (sortDirection == "asc")
            {
                distinctList = distinctList.OrderBy(x => x.GetType().GetProperty(sortBy)?.GetValue(x)).ToList();
            }

            serviceResult.SuccessState = true;
            serviceResult.Data = new
            {
                Data = distinctList.Skip((pageNum - 1) * pageSize).Take(pageSize),
                Summary = new
                {
                    Count = distinctList.Count
                }
            };

            return serviceResult;
        }

        public ServiceResult GetExerciseAttempTableStruct()
        {
            var properties = typeof(ExerciseAttempMongo).GetProperties();

            var res = new List<Dictionary<string, string>>();

            foreach (var property in properties)
            {
                var propertyDisplayName = property.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault() as DisplayNameAttribute;
                if (property.IsDefined(typeof(DisplayColumn), false))
                {
                    var x = new Dictionary<string, string>();
                    x.Add("FieldName", property.Name);
                    x.Add("DisplayName", propertyDisplayName?.DisplayName ?? "");

                    var typeName = property.PropertyType.Name;
                    if (property.PropertyType == typeof(DateTime?))
                    {
                        typeName = "DateTime";
                    }
                    if (property.PropertyType == typeof(int?))
                    {
                        typeName = "int";
                    }
                    if (property.PropertyType == typeof(long?))
                    {
                        typeName = "long";
                    }
                    x.Add("Type", typeName);
                    res.Add(x);
                }
            }

            serviceResult.SuccessState = true;
            serviceResult.Data = res;

            return serviceResult;
        }

        public ServiceResult GetReportPartitionByAttemps(long exerciseId)
        {
            var entity = new ExerciseAttempMongo
            {
                ExerciseID = exerciseId
            };

            var filters = new List<FilterObject>();

            filters.Add(new FilterObject
            {
                FieldName = nameof(ExerciseAttempMongo.ExerciseID),
                Operator = (int)Operator.EQUALS
            });

            var (list, count) = ((IExerciseRepository)_repository).QueryExerciseAttempList(entity, filters);

            var countList = list.GroupBy(x => x.UserID).Select(g =>
            {
                var dict = new Dictionary<string, object>();

                dict.Add("UserID", g.Key);
                dict.Add("Count", g.Count());

                return dict;
            }).ToList();

            var partitionAttemps = countList.GroupBy(x => { x.TryGetValue("Count", out var count); return count; }).Select(g =>
            {
                var dict = new Dictionary<string, object>();

                dict.Add("Attemps", g.Key);
                dict.Add("Count", g.Count());

                return dict;
            }).ToList();

            serviceResult.SuccessState = true;
            serviceResult.Data = partitionAttemps;

            return serviceResult;
        }

        public ServiceResult GetScoreSpectrum(long exerciseId)
        {
            var entity = new ExerciseAttempMongo
            {
                ExerciseID = exerciseId
            };

            var filters = new List<FilterObject>();

            filters.Add(new FilterObject
            {
                FieldName = nameof(ExerciseAttempMongo.ExerciseID),
                Operator = (int)Operator.EQUALS
            });

            var (list, count) = ((IExerciseRepository)_repository).QueryExerciseAttempList(entity, filters);

            var distinctList = list.GroupBy(x => x.UserID).Select(x => x.First()).ToList();

            var resList = distinctList.Select(x =>
            {
                if (x.TotalAnswerCount == null || x.CorrectAnswerCount == null)
                {
                    x.Score = 0;
                    return x;
                }

                decimal score = ((decimal)(x.CorrectAnswerCount ?? 0)) / ((decimal)(x.TotalAnswerCount ?? 1)) * 10;

                x.Score = ((int)Math.Round(score)) * 10;

                return x;
            }).GroupBy(x => x.Score).Select(g =>
            {
                var dict = new Dictionary<string, object>();

                dict.Add("Score", g.Key);
                dict.Add("Count", g.Count());

                return dict;
            }).ToList();

            resList = resList.OrderBy(x => x["Score"]).ToList();

            serviceResult.SuccessState = true;
            serviceResult.Data = resList;

            return serviceResult;
        }

        public ServiceResult DoExerciseRatioValue(long exerciseId)
        {
            var list = ((IExerciseRepository)_repository).GetAllUsersDidExercise(exerciseId);

            var exercise = _repository.GetEntityById(exerciseId);

            var lesson = _lessonRepository.GetEntityById(exercise.LessonID ?? 0);

            var total = lesson.LearnerCount;

            serviceResult.SuccessState = true;
            serviceResult.Data = ((decimal)(list?.Count ?? 0)) / (total ?? 1) * 100;

            return serviceResult;
        }
    }
}

