using Dapper;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossLang.Library;

namespace CrossLang.Infrastructure
{
    /// <summary>
    /// Refresh Token Repository kết nối database
    /// </summary>
    /// CREATEDBY: VMHOANG
    public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
    {
        #region Constructor
        public RefreshTokenRepository(IDBContext dbContext, SessionData sessionData) : base(dbContext, sessionData)
        {
        }
        #endregion

        #region Methods
        public int DeleteByUserId(long userId)
        {
            var rowsAffected = 0;

            _dbConnection.Open();
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    var parameters = new DynamicParameters();

                    parameters.Add($"@UserId", userId, DbType.String);

                    //Thực thi procedure
                    rowsAffected = _dbConnection.Execute($"{RefreshToken.BuildDeleteQueryStatic($"UserID = @UserId")}", parameters,
                        commandType: CommandType.Text, transaction: transaction);

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return rowsAffected;
        }
        #endregion
    }
}
