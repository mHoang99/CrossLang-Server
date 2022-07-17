using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySqlConnector;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace CrossLang.DBHelper
{   
    /// <summary>
    /// Class quản lý connection
    /// </summary>
    /// CREATED_BY: vmhoang
    public class DBContext : IDBContext
    {
        #region Properties
        
        /// <summary>
        /// connection tới db
        /// </summary>
        private IDbConnection DbConnection { get; }
        #endregion

        #region Constructor
        /// <summary>
        /// constructor
        /// </summary>
        public DBContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("LocalDB");
            DbConnection = new MySqlConnection(connectionString);
        }
        #endregion

        #region Methods
        public IDbConnection GetConnection()
        {
            DbConnection.Open();
            return DbConnection;
        }
        #endregion
    }
}
