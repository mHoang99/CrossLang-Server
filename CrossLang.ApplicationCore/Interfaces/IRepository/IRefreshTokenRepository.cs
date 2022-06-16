using CrossLang.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossLang.ApplicationCore.Interfaces
{
    public interface IRefreshTokenRepository : IBaseRepository<RefreshToken>
    {
        /// <summary>
        /// Xóa tất cả bản ghi bằng UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int DeleteByUserId(long userId);
    }
}
