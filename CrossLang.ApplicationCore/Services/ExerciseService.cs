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
                foreach (var question in exercise.Questions)
                {
                    question.Options.Shuffle();
                    question.Answers = null;
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
                if(entity.AttempAnswers == null)
                {
                    break;
                }

                var attempAnswer = entity.AttempAnswers.Find(x => x.QuestionID.Equals(question.ID));
                if(attempAnswer?.Answers.Count() != question.Answers.Count())
                {
                    break;
                }

                foreach (var answer in question.Answers)
                {
                    if(!attempAnswer.Answers.Exists(x => x.Equals(answer)))
                    {
                        correctAnswers = correctAnswers - 1;
                        break ;
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
    }
}

