using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.API.Models.Requests;
using CrossLang.ApplicationCore.Entities;
using CrossLang.Models;
using CrossLang.Library;
using CrossLang.Authentication.JWT;

namespace CrossLang.API.Controllers
{
    //[EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _config;
        private readonly AccessTokenGenerator _accessTokenGenerator;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;
        private readonly RefreshTokenValidator _refreshTokenValidator;
        private readonly SessionData _sessionData;

        public AuthController(
            IAuthService authService,
            IConfiguration config,
            AccessTokenGenerator jwtGenerator,
            RefreshTokenGenerator refreshTokenGenerator,
            RefreshTokenValidator refreshTokenValidator,
            SessionData sessionData
            )
        {
            _authService = authService;
            _config = config;
            _accessTokenGenerator = jwtGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _refreshTokenValidator = refreshTokenValidator;
            _sessionData = sessionData;
        }

        /// <summary>
        /// Xác thực
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        async public Task<IActionResult> Authenticate([FromBody] UserLogin entity)
        {
            //Gọi service xác thực tài khoản
            var response = _authService.Authenticate(entity);

            //Tài khoản hợp lệ
            if (response.SuccessState)
            {
                var user = response.Data as User;

                //Tạo token mới từ user nhận được
                var tokenString = _accessTokenGenerator.GenerateToken(user);
                var refreshTokenString = _refreshTokenGenerator.GenerateToken();

                //Lưu refresh token
                var addRes = _authService.AddRefreshToken(refreshTokenString, user.ID);

                if (addRes.SuccessState)
                {
                    addRes.Data = new
                    {
                        user = user,
                        accessToken = tokenString,
                        refreshToken = refreshTokenString
                    };
                    return Ok(addRes.ConvertToApiReturn());
                }
                else
                {
                    return Ok(addRes.ConvertToApiReturn());
                }
            }
            //Tài khoản không hợp lệ
            else
            {
                return Ok(response.ConvertToApiReturn());
            }
        }

        /// <summary>
        /// Lấy access token mới
        /// </summary>
        /// <param name="refreshRequest"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("refresh")]
        async public Task<IActionResult> Refresh([FromBody] RefreshTokenRequest refreshRequest)
        {
            //Validate refresh token
            bool isValidRefreshToken = _refreshTokenValidator.Validate(refreshRequest.RefreshToken);
            if (!isValidRefreshToken)
            {
                return Unauthorized();
            }

            //Lấy thông tin user tương ứng với token
            var serviceRes = _authService.getRefreshTokenOwner(refreshRequest.RefreshToken);

            if (!serviceRes.SuccessState)
            {
                return Unauthorized();
            }

            serviceRes.Data = new
            {
                AccessToken = _accessTokenGenerator.GenerateToken(serviceRes.Data as User)
            };

            //Trả về token mới
            return Ok(serviceRes.ConvertToApiReturn());
        }

        /// <summary>
        /// Thoát đăng nhập
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("logout")]
        async public Task<IActionResult> Logout()
        {
            //Lấy UserId từ trong request
            int.TryParse(HttpContext.User.FindFirst("id").Value, out var userId);

            //Xóa hết refresh token tương ứng vs id trong db
            var res = _authService.DeleteRefreshTokenByUserId(userId);

            return Ok(res.ConvertToApiReturn());
        }

        /// <summary>
        /// Lấy ra user tương ứng với access token hiện tại
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        async public Task<IActionResult> Get()
        {
            int.TryParse(HttpContext.User.FindFirstValue("id"), out var userId);
            var res = _authService.GetUserById(userId);

            if (!res.SuccessState)
            {
                return Unauthorized(res.ConvertToApiReturn());
            }

            return Ok(res.ConvertToApiReturn());
        }
    }
}
