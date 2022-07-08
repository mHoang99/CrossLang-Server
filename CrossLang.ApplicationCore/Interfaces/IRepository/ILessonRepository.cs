using System;
using CrossLang.ApplicationCore.Entities;
using CrossLang.Models;
using MongoDB.Driver;

namespace CrossLang.ApplicationCore.Interfaces.IRepository
{
	public interface ILessonRepository : IBaseRepository<Lesson>
{	
		void InsertLessonContentMongo(LessonContentMongo entity);
		List<LessonContentMongo> GetLessonContentMongo(long lessonID);
		DeleteResult DeleteLessonContentMongo(long lessonID);
		ReplaceOneResult UpsertLessonContentMongo(long masterID, LessonContentMongo entity);
        List<DictionaryWord> GetRelatedWords(long id);
		List<ExerciseAttempMongo> GetAttempHistory(long lessonId);
		IEnumerable<IDictionary<string, object>> GetLearningLessonList();
		IEnumerable<IDictionary<string, object>> GetLessonList(Lesson entity, List<FilterObject> filters, string formula, string sortBy, string sortDirection, int pageNum, int pageSize);
    }
}

