using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrossLang.Authentication.JWT
{
    /// <summary>
    /// Class tạo refresh token
    /// </summary>
    /// CREATED_BY: vmhoang
    public class RefreshTokenGenerator : TokenGenerator
    {
        public RefreshTokenGenerator(IConfiguration config) : base(
            config["JwtConfig:RefreshTokenSecret"],
            config["JwtConfig:Issuer"],
            config["JwtConfig:Audience"],
            config["JwtConfig:RefreshTokenExpirationMinutes"],
            false
        )
        { }
    }
}
