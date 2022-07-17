using System;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Interfaces.IRepository;
using CrossLang.DBHelper;
using CrossLang.Library;

namespace CrossLang.Infrastructure
{
    public class UpgradeCodeRepository : BaseRepository<UpgradeCode>, IUpgradeCodeRepository
    {
        public UpgradeCodeRepository(IDBContext dbContext, SessionData sessionData) : base(dbContext, sessionData)
        {
        }
    }
}

