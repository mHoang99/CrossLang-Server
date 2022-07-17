using System;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Interfaces.IRepository;
using CrossLang.DBHelper;
using CrossLang.Library;

namespace CrossLang.Infrastructure
{
    public class LessonCommentRepository : BaseRepository<LessonComment>, ILessonCommentRepository
    {
        public LessonCommentRepository(IDBContext dbContext, SessionData sessionData) : base(dbContext, sessionData)
        {
        }
    }
}

