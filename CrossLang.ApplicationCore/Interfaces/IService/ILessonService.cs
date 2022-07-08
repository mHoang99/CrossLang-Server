using System;
using CrossLang.ApplicationCore.Entities;
using CrossLang.Models;

namespace CrossLang.ApplicationCore.Interfaces.IService
{
    public interface ILessonService : IBaseService<Lesson>
    {
        ServiceResult GetAttempHistory(long id);
        ServiceResult FinishLesson(Lesson entity);
        ServiceResult GetLearningLessonList();
        ServiceResult GetLessonList(Lesson entity, List<FilterObject> filters, string formula, string sortBy, string sortDirection, int pageNum, int pageSize);
    }
}

