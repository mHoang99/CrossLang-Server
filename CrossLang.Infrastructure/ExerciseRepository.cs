using System;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Interfaces.IRepository;
using CrossLang.Library;
using MongoDB.Driver;

namespace CrossLang.Infrastructure
{
    public class ExerciseRepository : BaseRepository<Exercise>, IExerciseRepository
    {
        private IMongoDBContext _mongoDBContext;

        public ExerciseRepository(IDBContext dbContext, SessionData sessionData, IMongoDBContext mongoDBContext) : base(dbContext, sessionData)
        {
            _mongoDBContext = mongoDBContext;
        }

        public void InsertQuestionsMongo(List<QuestionMongo> entities)
        {
            var collection = _mongoDBContext.GetCollection<QuestionMongo>();

            collection.InsertMany(entities);
        }

        public void InsertQuestionMongo(QuestionMongo entitiy)
        {
            var collection = _mongoDBContext.GetCollection<QuestionMongo>();

            collection.InsertOne(entitiy);
        }


        public List<QuestionMongo> GetExerciseQuestionsMongo(long exerciseID)
        {
            var collection = _mongoDBContext.GetCollection<QuestionMongo>();
            var filter = Builders<QuestionMongo>.Filter.Eq(nameof(QuestionMongo.ExerciseID), exerciseID);
            return collection.FindSync<QuestionMongo>(filter).ToList();
        }

        public DeleteResult DeleteExerciseQuestionsMongo(long exerciseID)
        {
            var collection = _mongoDBContext.GetCollection<QuestionMongo>();
            return collection.DeleteOne(x => x.ExerciseID == exerciseID);
        }

        public ReplaceOneResult UpsertLessonContentMongo(long masterID, QuestionMongo entity)
        {
            var collection = _mongoDBContext.GetCollection<QuestionMongo>();
            var filter = Builders<QuestionMongo>.Filter.Eq(nameof(QuestionMongo.ExerciseID), masterID);
            return collection.ReplaceOne(filter, entity, new ReplaceOptions { IsUpsert = true });
        }
    }
}

