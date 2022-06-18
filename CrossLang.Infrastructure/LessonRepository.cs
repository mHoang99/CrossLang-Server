using System;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Interfaces.IRepository;
using CrossLang.Library;
using Dapper;
using MongoDB.Driver;

namespace CrossLang.Infrastructure
{
    public class LessonRepository : BaseRepository<Lesson>, ILessonRepository
    {
        private IMongoDBContext mongoDBContext;

        public LessonRepository(IDBContext dbContext, SessionData sessionData, IMongoDBContext mDBContext) : base(dbContext, sessionData)
        {
            mongoDBContext = mDBContext;
        }

        public void InsertLessonContentMongo(LessonContentMongo entity)
        {
            var collection = this.mongoDBContext.GetCollection<LessonContentMongo>();

            collection.InsertOne(entity);
        }

        public List<LessonContentMongo> GetLessonContentMongo(long lessonID)
        {
            var collection = this.mongoDBContext.GetCollection<LessonContentMongo>();
            var filter = Builders<LessonContentMongo>.Filter.Eq(nameof(LessonContentMongo.LessonID), lessonID);
            return collection.FindSync<LessonContentMongo>(filter).ToList();
        }

        public DeleteResult DeleteLessonContentMongo(long lessonID)
        {
            var collection = this.mongoDBContext.GetCollection<LessonContentMongo>();
            return collection.DeleteOne(x => x.LessonID == lessonID);
        }

        public ReplaceOneResult UpsertLessonContentMongo(long masterID, LessonContentMongo entity)
        {
            var collection = this.mongoDBContext.GetCollection<LessonContentMongo>();
            var filter = Builders<LessonContentMongo>.Filter.Eq(nameof(LessonContentMongo.LessonID), masterID);
            return collection.ReplaceOne(filter, entity, new ReplaceOptions { IsUpsert = true });
        }

        public List<DictionaryWord> GetRelatedWords(long id)
        {
            var res = _dbConnection.Query<DictionaryWord>(
           "Select dw.*, fc.ID as FlashCardID, fcc.ID as FlashCardCollectionID FROM flash_card_collection fcc JOIN flash_card fc ON fc.FlashCardCollectionID = fcc.ID JOIN dictionary_word dw on dw.ID = fc.DictionaryWordID WHERE fcc.LessonID = @ID",
           new
           {
               @ID = id
           }
           )?.ToList();

            //.ToDictionary(pair => pair.Key, pair => pair.Value);

            return res ?? new List<DictionaryWord>();
        }
    }
}

