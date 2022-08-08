using System;
using System.Data;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Interfaces.IRepository;
using CrossLang.DBHelper;
using CrossLang.Library;
using CrossLang.Models;
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

        public ReplaceOneResult UpdateLessonContentMongo(long masterID, LessonContentMongo entity)
        {
            var collection = this.mongoDBContext.GetCollection<LessonContentMongo>();
            var filter = Builders<LessonContentMongo>.Filter.Eq(nameof(LessonContentMongo.LessonID), masterID);
            return collection.ReplaceOne(filter, entity);
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

        public override IEnumerable<IDictionary<string, object>> GetDetailsById(long id)
        {

            var dicts = _dbConnection.Query(
            "Select l.*, uls.IsFinished, lc.Name as LessonCategoryName from lesson l LEFT JOIN lesson_category lc on lc.ID = l.LessonCategoryID LEFT JOIN user_lesson_status uls  ON uls.LessonID = l.ID  AND uls.UserID = @UserID WHERE l.ID = @ID;",
            new
            {
                @ID = id,
                @UserID = _sessionData.ID
            }
            ).Cast<IDictionary<string, object>>();

            //.ToDictionary(pair => pair.Key, pair => pair.Value);

            return dicts;
        }


        public override IEnumerable<IDictionary<string, object>> GetPreviewById(long id)
        {
            var dicts = _dbConnection.Query(
            "SELECT l.*, u.Username, u.DateOfBirth, u.FullName, u.Gender, u.PersonalImage, u.Description as PersonalDescription, uls.IsFinished, ex.ID as ExerciseID, lc.Name as CategoryName FROM lesson l LEFT JOIN lesson_category lc on l.LessonCategoryID = lc.ID LEFT JOIN user u on l.UserID = u.ID LEFT JOIN user_lesson_status uls on l.ID = uls.LessonID AND uls.UserID = @UserID  LEFT JOIN exercise ex ON ex.LessonID = l.ID  WHERE l.ID = @ID",
            new
            {
                @ID = id,
                @UserID = _sessionData.ID
            }
            ).Cast<IDictionary<string, object>>();

            return dicts;
        }

        public List<ExerciseAttempMongo> GetAttempHistory(long lessonId)
        {
            var collection = this.mongoDBContext.GetCollection<ExerciseAttempMongo>();
            var exerciseFilter = Builders<ExerciseAttempMongo>.Filter.Eq(nameof(ExerciseAttempMongo.LessonID), lessonId);
            var userFilter = Builders<ExerciseAttempMongo>.Filter.Eq(nameof(ExerciseAttempMongo.UserID), _sessionData.ID);
            var combineFilter = Builders<ExerciseAttempMongo>.Filter.And(exerciseFilter, userFilter);

            return collection.FindSync<ExerciseAttempMongo>(combineFilter).ToList();
        }

        public IEnumerable<IDictionary<string, object>> GetLearningLessonList()
        {
            var lessons = _dbConnection.Query(
            "Select l.*, u.Username, u.FullName, u.Avatar from lesson l JOIN user_lesson_status uls ON l.ID = uls.LessonID AND uls.UserID = @UserID JOIN user u ON l.UserID = u.ID WHERE uls.IsFinished IS NOT TRUE LIMIT 20;",
            new
            {
                @UserID = _sessionData.ID
            }
            ).Cast<IDictionary<string, object>>();

            return lessons;
        }

        public IEnumerable<IDictionary<string, object>> GetLessonList(Lesson entity, List<FilterObject> filters, string formula, string sortBy, string sortDirection, int pageNum, int pageSize)
        {
            var parameters = new DynamicParameters();

            var filterStr = BuildFilterString(entity, filters, formula);

            filters.ForEach(x =>
            {
                MappingDbTypeByField(entity, x.FieldName, ref parameters);
            });


            var query = $"SELECT * FROM (SELECT l.*, u.Username, u.FullName, u.Avatar, uls.IsFinished, uls.UserID as LearnerID from lesson l JOIN user_lesson_status uls ON l.ID = uls.LessonID AND uls.UserID = @LearnerID JOIN user u ON l.UserID = u.ID) AS T WHERE {filterStr} ORDER BY {sortBy} {sortDirection} LIMIT {pageSize} OFFSET {pageSize * (pageNum - 1)};";


            var lessons = _dbConnection.Query(query
            ,
            parameters
            ).Cast<IDictionary<string, object>>();

            return lessons;
        }

        public override List<Lesson> QueryList(Lesson entity, List<FilterObject> filters, string formula, string sortBy, string sortDirection, int pageNum, int pageSize)
        {
            var parameters = new DynamicParameters();

            var filterStr = BuildFilterString(entity, filters, formula);

            filters.ForEach(x =>
            {
                MappingDbTypeByField(entity, x.FieldName, ref parameters);
            });

            var res = new List<User>();


            var query = $"SELECT * FROM view_lesson T WHERE {filterStr} ORDER BY {sortBy} {sortDirection} LIMIT {pageSize} OFFSET {pageSize * (pageNum - 1)};";

            var resp = (_dbConnection.Query<Lesson>(query, parameters, commandType: CommandType.Text))?.ToList() ?? new List<Lesson>();

            return resp;
        }
    }
}

