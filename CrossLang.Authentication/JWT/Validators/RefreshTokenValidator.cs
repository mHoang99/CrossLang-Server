using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossLang.Authentication.JWT
{
    /// <summary>
    /// Class kiểm tra refresh token
    /// </summary>
    /// CREATED_BY: vmhoang
    public class RefreshTokenValidator
    {

        private readonly IConfiguration _config;

        public RefreshTokenValidator(IConfiguration config)
        {
            _config = config;
        }


        /// <summary>
        /// Xác thực token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns>true: Hợp lệ | false: Không hợp lệ</returns>
        /// CREATED_BY: vmhoang
        public bool Validate(string refreshToken)
        {
            //Thiết lập các params để xác thực
            var validationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtConfig:RefreshTokenSecret"])),
                ValidIssuer = _config["JwtConfig:Issuer"],
                ValidAudience = _config["JwtConfig:Audience"],
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                //validate token
                tokenHandler.ValidateToken(refreshToken, validationParameters, out SecurityToken validatedToken);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
