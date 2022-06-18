using System;
using CrossLang.ApplicationCore.Entities;
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
    }
}

