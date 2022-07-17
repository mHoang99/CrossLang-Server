using System;
using CrossLang.ApplicationCore.Entities;
using CrossLang.Models;

namespace CrossLang.ApplicationCore.Interfaces.IService
{

    public interface IExerciseService : IBaseService<Exercise>
    {
        public ServiceResult Submit(ExerciseAttempMongo entity);
        public ServiceResult GetAttempHistory(long exerciseId);
        public ServiceResult GetExerciseAttempTableStruct();
        public ServiceResult QueryExerciseAttempList(ExerciseAttempMongo entity, List<FilterObject> filters, string formula, string sortBy, string sortDirection, int pageNum, int pageSize);

        public ServiceResult GetReportPartitionByAttemps(long exerciseId);
        public ServiceResult GetScoreSpectrum(long exerciseId);
        public ServiceResult DoExerciseRatioValue(long exerciseId);
    }
}

