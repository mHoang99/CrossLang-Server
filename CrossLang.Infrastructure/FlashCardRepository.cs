using System;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Interfaces.IRepository;
using CrossLang.DBHelper;
using CrossLang.Library;

namespace CrossLang.Infrastructure
{
    public class FlashCardRepository : BaseRepository<FlashCard>, IFlashCardRepository
    {
        public FlashCardRepository(IDBContext dbContext, SessionData sessionData) : base(dbContext, sessionData)
        {
        }
    }
}

