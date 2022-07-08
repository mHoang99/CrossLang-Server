using System;
using CrossLang.ApplicationCore.Entities;
using CrossLang.Models;

namespace CrossLang.ApplicationCore.Interfaces.IService
{

    public interface IExerciseService : IBaseService<Exercise>
    {
        public ServiceResult Submit(ExerciseAttempMongo entity);
        public ServiceResult GetAttempHistory(long exerciseId);
    }
}

