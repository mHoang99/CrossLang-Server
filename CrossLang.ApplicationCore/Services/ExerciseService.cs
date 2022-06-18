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
    public class ExerciseService : BaseService<Exercise>, IExerciseService
    {
        public ExerciseService(IExerciseRepository repository, IHttpContextAccessor httpContextAccessor, SessionData sessionData) : base(repository, httpContextAccessor, sessionData)
        {
        }
        protected override void AfterAdd(ref Exercise entity)
        {
            base.AfterAdd(ref entity);

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
                exercise.Questions = ((IExerciseRepository)_repository).GetExerciseQuestionsMongo(id);
            }

            serviceResult.SuccessState = true;
            serviceResult.Data = exercise;

            return serviceResult;
        }
    }
}

