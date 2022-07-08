using System;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Interfaces.IRepository;
using CrossLang.Library;
using Dapper;

namespace CrossLang.Infrastructure
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IDBContext dbContext, SessionData sessionData) : base(dbContext, sessionData)
        {
        }

        public override IEnumerable<IDictionary<string, object>> GetDetailsById(long id)
        {
            var dicts = _dbConnection.Query(
            "Select * from user WHERE ID = @ID",
           new
           {
               @ID = id
           }
           ).Cast<IDictionary<string, object>>();

            return dicts;
        }
    }
}

