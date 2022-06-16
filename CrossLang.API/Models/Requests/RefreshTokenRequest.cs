using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CrossLang.API.Models.Requests
{
    /// <summary>
    /// Request body khi refresh token
    /// </summary>
    /// CREATED_BY: vmhoang
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; }
    }
}
