using System;
using CrossLang.ApplicationCore.Entities;
using MongoDB.Driver;

namespace CrossLang.ApplicationCore.Interfaces.IRepository
{
	public interface IExerciseRepository : IBaseRepository<Exercise>
	{

        public void InsertQuestionsMongo(List<QuestionMongo> entities);


        public void InsertQuestionMongo(QuestionMongo entitiy);


        public List<QuestionMongo> GetExerciseQuestionsMongo(long exerciseID);


        public DeleteResult DeleteExerciseQuestionsMongo(long exerciseID);


        public ReplaceOneResult UpsertLessonContentMongo(long masterID, QuestionMongo entity);

        public void InsertExerciseAttempMongo(ExerciseAttempMongo entity);

        public List<ExerciseAttempMongo> GetExerciseAttempMongo(long exerciseID);

        public List<ExerciseAttempMongo> GetAttempHistory(long exerciseId);
    }

}

