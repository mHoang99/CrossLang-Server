using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossLang.ApplicationCore.Interfaces
{
    public interface IDBContext
    {
        #region Methods
        IDbConnection GetConnection();
        #endregion
    }
}
