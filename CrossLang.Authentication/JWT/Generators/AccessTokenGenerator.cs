using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CrossLang.Authentication.JWT
{
    /// <summary>
    /// Class tạo access token
    /// </summary>
    /// CREATED_BY: vmhoang
    public class AccessTokenGenerator : TokenGenerator
    {
        public AccessTokenGenerator(IConfiguration config) : base(
            config["JwtConfig:AccessTokenSecret"],
            config["JwtConfig:Issuer"],
            config["JwtConfig:Audience"],
            config["JwtConfig:AccessTokenExpirationMinutes"],
            true
        )
        { }
    }
}
