using System;
using System.Data;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Interfaces.IRepository;
using CrossLang.DBHelper;
using CrossLang.Library;
using CrossLang.Models;
using Dapper;
using MongoDB.Bson;
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

        public List<QuestionMongo> GetExercisesQuestionsMongo(List<long> exerciseIDs)
        {
            var collection = _mongoDBContext.GetCollection<QuestionMongo>();
            var filter = Builders<QuestionMongo>.Filter.In(nameof(QuestionMongo.ExerciseID), exerciseIDs);
            return collection.FindSync<QuestionMongo>(filter).ToList();
        }

        public DeleteResult DeleteExerciseQuestionsMongo(long exerciseID)
        {
            var collection = _mongoDBContext.GetCollection<QuestionMongo>();
            return collection.DeleteMany(x => x.ExerciseID == exerciseID);
        }

        public ReplaceOneResult UpsertLessonContentMongo(long masterID, QuestionMongo entity)
        {
            var collection = _mongoDBContext.GetCollection<QuestionMongo>();
            var filter = Builders<QuestionMongo>.Filter.Eq(nameof(QuestionMongo.ExerciseID), masterID);
            return collection.ReplaceOne(filter, entity, new ReplaceOptions { IsUpsert = true });
        }




        public void InsertExerciseAttempMongo(ExerciseAttempMongo entity)
        {
            entity.CreatedDate = DateTime.Now;

            var collection = this._mongoDBContext.GetCollection<ExerciseAttempMongo>();

            collection.InsertOne(entity);
        }

        public List<ExerciseAttempMongo> GetExerciseAttempMongo(long exerciseID)
        {
            var collection = this._mongoDBContext.GetCollection<ExerciseAttempMongo>();
            var filter = Builders<ExerciseAttempMongo>.Filter.Eq(nameof(ExerciseAttempMongo.ExerciseID), exerciseID);
            return collection.FindSync<ExerciseAttempMongo>(filter).ToList();
        }

        public List<ExerciseAttempMongo> GetAttempHistory(long exerciseId)
        {
            var collection = this._mongoDBContext.GetCollection<ExerciseAttempMongo>();
            var exerciseFilter = Builders<ExerciseAttempMongo>.Filter.Eq(nameof(ExerciseAttempMongo.ExerciseID), exerciseId);
            var userFilter = Builders<ExerciseAttempMongo>.Filter.Eq(nameof(ExerciseAttempMongo.UserID), _sessionData.ID);
            var combineFilter = Builders<ExerciseAttempMongo>.Filter.And(exerciseFilter, userFilter);

            return collection.FindSync<ExerciseAttempMongo>(combineFilter).ToList();
        }

        public override (List<Exercise>, long) QueryList(Exercise entity, List<FilterObject> filters, string formula, string sortBy, string sortDirection, int pageNum, int pageSize)
        {
            var parameters = new DynamicParameters();

            var filterStr = BuildFilterString(entity, filters, formula);

            filters.ForEach(x =>
            {
                MappingDbTypeByField(entity, x.FieldName, ref parameters);
            });

            var res = new List<Exercise>();


            var query = $"SELECT * FROM view_exercise T WHERE {filterStr} ORDER BY {sortBy} {sortDirection} LIMIT {pageSize} OFFSET {pageSize * (pageNum - 1)};";

            var resp = (_dbConnection.Query<Exercise>(query, parameters, commandType: CommandType.Text))?.ToList() ?? new List<Exercise>();

            var queryCount = $"SELECT COUNT(*) FROM view_exercise WHERE {filterStr};";

            var total = (_dbConnection.ExecuteScalar<long>(queryCount, parameters, commandType: CommandType.Text));

            return (resp, total);
        }

        public (List<ExerciseAttempMongo>, int) QueryExerciseAttempList(ExerciseAttempMongo entity, List<FilterObject> filters)
        {
            var collection = this._mongoDBContext.GetCollection<ExerciseAttempMongo>();

            var orFilters = new List<FilterDefinition<ExerciseAttempMongo>>();


            

            var exerciseFilter = Builders<ExerciseAttempMongo>.Filter.Eq(nameof(ExerciseAttempMongo.ExerciseID), entity.ExerciseID);

            //var combineFilter = Builders<ExerciseAttempMongo>.Filter.And(searchFilter, exerciseFilter);

            var list = collection.FindSync<ExerciseAttempMongo>(exerciseFilter).ToList();


            var users = getUsers(list.Select(x => x.UserID ?? 0).ToList());

            foreach (var item in list)
            {
                var user = users.Find(x => x.ID == item.UserID);
                if (user != null)
                {
                    item.Username = user.Username;
                    item.Fullname = user.FullName;
                }
            }

            var filteredList = list.FindAll(x =>
            {
                foreach (var filter in filters)
                {

                    if (filter.FieldName != "ExerciseID")
                    {
                        var valueProp = entity.GetType().GetProperty(filter.FieldName);
                        var xProp = entity.GetType().GetProperty(filter.FieldName);


                        if (valueProp != null)
                        {
                            var valueStr = valueProp.GetValue(entity)?.ToString() ?? "";
                            var xStr = valueProp.GetValue(x)?.ToString() ?? "";

                            if(valueStr.StartsWith("%"))
                            {
                                valueStr = valueStr.Substring(1, valueStr.Length - 2);
                            }

                            if(xStr.Contains(valueStr))
                            {
                                return true;
                            }
                        }
                    }


                    if (filter.FieldName == "ExerciseID" && filters.Count() == 1)
                    {
                        return true;
                    }
                }

                return false;
            });

            var resList = from num in filteredList
                               orderby num.CorrectAnswerCount descending
                               select num;

            var listCount = resList.Count();



            return (resList.ToList(), listCount);
        }


        public override Exercise GetEntityById(long id)
        {
            var query = $"SELECT * FROM view_exercise WHERE ID = {id}";

            //Khởi tạo commandText
            var entity = _dbConnection.QueryFirstOrDefault<Exercise>(
                query,
                commandType: CommandType.Text
                );

            return entity;
        }

        public List<long> GetAllUsersDidExercise(long exerciseId)
        {
            var collection = this._mongoDBContext.GetCollection<ExerciseAttempMongo>();
            var exerciseFilter = Builders<ExerciseAttempMongo>.Filter.Eq(nameof(ExerciseAttempMongo.ExerciseID), exerciseId);

            return collection.FindSync(exerciseFilter).ToList().Select(x => x.UserID ?? 0).Distinct().ToList();
        }
    }
}

