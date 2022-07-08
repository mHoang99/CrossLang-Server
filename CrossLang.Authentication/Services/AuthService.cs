using CrossLang.ApplicationCore;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Entities;
using CrossLang.Models;
using CrossLang.Library;
using CrossLang.Library.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CrossLang.Authentication
{
    /// <summary>
    /// Service xử lý authentication
    /// </summary>
    public class AuthService : BaseService<RefreshToken>, IAuthService
    {
        #region Fields
        private IBaseRepository<User> _userRepository;

        private IHttpContextAccessor _httpContextAccessor;

        private new IRefreshTokenRepository _repository;
        #endregion

        #region Constructor
        public AuthService(
           IRefreshTokenRepository repository,
           IBaseRepository<User> userRepo,
           IHttpContextAccessor httpContextAccessor, SessionData sessionData)
           : base(repository, httpContextAccessor, sessionData)
        {
            _userRepository = userRepo;
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Methods
        public ServiceResult AddRefreshToken(string refreshTokenString, long userId)
        {
            try
            {
                var entity = new RefreshToken
                {
                    HashedValue = CLHasher.Sha256Hash(refreshTokenString),
                    UserId = userId
                };

                serviceResult.SuccessState = true;
                serviceResult.Data = _repository.Add(entity);

                //Không tác động được bản ghi
                if (int.Parse(serviceResult.Data.ToString()) <= 0)
                {
                    serviceResult = RowAffectingUnexpectedFailureResponse();
                }

                return serviceResult;
            }
            catch (Exception ex)
            {
                throw new ServiceException(
                    ex.Message,
                    UnexpectedErrorResponse(ex.Message)
                );
            }
        }

        public ServiceResult Authenticate(UserLogin entity)
        {
            try
            {
                var tmpUser = new User
                {
                    Username = entity.Username
                };

                var usernameProp = tmpUser.GetType().GetProperty("Username");

                var user = _userRepository.GetEntityByProperty(tmpUser, usernameProp);

                //Không tìm thấy user
                if (user == null)
                {
                    serviceResult.UserMsg = Properties.Resources.CrossLang_Authentication_ResponseMessage_UserNotFound;
                    serviceResult.DevMsg = serviceResult.UserMsg;
                    serviceResult.SuccessState = false;
                    return serviceResult;
                }

                //Verify Password
                if (!CLHasher.BCryptVerify(entity.Password, user.Password))
                {
                    serviceResult.UserMsg = Properties.Resources.CrossLang_Authentication_ResponseMessage_WrongPassword;
                    serviceResult.DevMsg = serviceResult.UserMsg;
                    serviceResult.SuccessState = false;
                    return serviceResult;
                }

                serviceResult.SuccessState = true;
                serviceResult.Data = user;
                return serviceResult;
            }
            catch (Exception ex)
            {
                throw new ServiceException(
                    ex.Message,
                    UnexpectedErrorResponse(ex.Message)
                );
            }
        }

        public ServiceResult DeleteRefreshTokenByUserId(long userId)
        {
            try
            {
                serviceResult.SuccessState = true;
                serviceResult.Data = _repository.DeleteByUserId(userId);


                return serviceResult;
            }
            catch (Exception ex)
            {
                throw new ServiceException(
                    ex.Message,
                    UnexpectedErrorResponse(ex.Message)
                );
            }
        }

        public ServiceResult getRefreshTokenOwner(string refreshTokenString)
        {
            try
            {
                //Hash token để so sánh vs db
                var tmpToken = new RefreshToken
                {
                    HashedValue = CLHasher.Sha256Hash(refreshTokenString)
                };

                var hashedValueProperty = tmpToken.GetType().GetProperty("HashedValue");

                //Gọi repository tìm token
                var currentToken = _repository.GetEntityByProperty(tmpToken, hashedValueProperty);

                if (currentToken == null)
                {
                    //lỗi
                    serviceResult.UserMsg = Properties.Resources.CrossLang_Authentication_ResponseMessage_TokenNotFound;
                    serviceResult.DevMsg = serviceResult.UserMsg;
                    serviceResult.SuccessState = false;
                    return serviceResult;
                }

                //Gọi repository lấy thông tin user
                var user = _userRepository.GetEntityById(currentToken.UserId);
                if (user == null)
                {
                    //Lỗi
                    serviceResult.UserMsg = Properties.Resources.CrossLang_Authentication_ResponseMessage_UserNotFound;
                    serviceResult.DevMsg = serviceResult.UserMsg;
                    serviceResult.SuccessState = false;
                    return serviceResult;
                }

                serviceResult.SuccessState = true;
                serviceResult.Data = user;

                return serviceResult;
            }
            catch (Exception ex)
            {
                throw new ServiceException(
                    ex.Message,
                    UnexpectedErrorResponse(ex.Message)
                );
            }
        }

        public ServiceResult GetUserById(long id)
        {
            try
            {

                serviceResult.SuccessState = true;
                serviceResult.Data = _userRepository.GetEntityById(id);

                return serviceResult;
            }
            catch (Exception ex)
            {
                throw new ServiceException(
                    ex.Message,
                    UnexpectedErrorResponse(ex.Message)
                );
            }
        }

        public ServiceResult Register(User entity)
        {
            var tmpUser = new User
            {
                Username = entity.Username
            };

            var usernameProp = tmpUser.GetType().GetProperty("Username");

            var resUserByUsername = _userRepository.GetEntityByProperty(tmpUser, usernameProp);

            if (resUserByUsername != null)
            {
                this.serviceResult.SuccessState = false;
                serviceResult.UserMsg = Properties.Resources.CrossLang_Authentication_ResponseMessage_Duplicate_Username;
                return serviceResult;
            }


            var tmpUserEmail = new User
            {
                Email = entity.Email
            };

            var emailProp = tmpUserEmail.GetType().GetProperty("Email");

            var resUserByEmail = _userRepository.GetEntityByProperty(tmpUserEmail, emailProp);

            if (resUserByEmail != null)
            {
                this.serviceResult.SuccessState = false;
                serviceResult.UserMsg = Properties.Resources.CrossLang_Authentication_ResponseMessage_Duplicate_Email;
                return serviceResult;
            }

            serviceResult.SuccessState = true;
            return serviceResult;

        }
        #endregion
    }
}
