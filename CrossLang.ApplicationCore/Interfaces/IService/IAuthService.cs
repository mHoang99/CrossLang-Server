using CrossLang.ApplicationCore.Entities;
using CrossLang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossLang.ApplicationCore.Interfaces
{
    public interface IAuthService : IBaseService<RefreshToken>
    {
        /// <summary>
        /// Xác thực người dùng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// CREATEDBY: VMHOANG
        ServiceResult Authenticate(UserLogin entity);

        /// <summary>
        /// Lấy người dùng hiện tại theo id
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// CREATEDBY: VMHOANG
        ServiceResult GetUserById(long id);

        /// <summary>
        /// Thêm refresh token vào database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// CREATEDBY: VMHOANG
        ServiceResult AddRefreshToken(string refreshTokenString, long userId);

        /// <summary>
        /// Xóa tất cả RefreshToken liên quan tới người dùng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// CREATEDBY: VMHOANG
        ServiceResult DeleteRefreshTokenByUserId(long userId);

        /// <summary>
        /// Lấy người dùng sở hữu refresh token
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// CREATEDBY: VMHOANG
        ServiceResult getRefreshTokenOwner(string refreshToken);
    }
}
