using System;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Interfaces.IRepository;
using CrossLang.ApplicationCore.Interfaces.IService;
using CrossLang.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CrossLang.ApplicationCore.Services
{
    public class UserService : BaseService<User>, IUserService
    {

        private readonly IConfiguration _configuration;

        public UserService(IUserRepository repository, IHttpContextAccessor httpContextAccessor, SessionData sessionData, IConfiguration configuration) : base(repository, httpContextAccessor, sessionData)
        {
            _configuration = configuration;
        }

        protected override void BeforeAdd(ref User entity)
        {
            entity.CreatedDate = DateTime.Now;
            entity.ModifiedDate = DateTime.Now;
            entity.RoleID = 3;
            entity.Package = Enums.PackageEnum.Free;
            entity.UserPermission = 0;
            entity.Avatar = _configuration["DefaultAvatar"];
            entity.Password = CLHasher.BcryptHash(entity.RegisterPassword);
        }

        protected override bool CustomValidate(User entity, List<string>? fields)
        {
            var tmpUser = new User
            {
                Username = entity.Username
            };

            var usernameProp = tmpUser.GetType().GetProperty("Username");

            var resUserByUsername = _repository.GetEntityByProperty(tmpUser, usernameProp);

            if (resUserByUsername != null)
            {
                this.serviceResult.SuccessState = false;
                serviceResult.UserMsg = Properties.Resources.CrossLang_ResponseMessage_Duplicate_Username;
                return false;
            }


            var tmpUserEmail = new User
            {
                Email = entity.Email
            };

            var emailProp = tmpUserEmail.GetType().GetProperty("Email");

            var resUserByEmail = _repository.GetEntityByProperty(tmpUserEmail, emailProp);

            if (resUserByEmail != null)
            {
                this.serviceResult.SuccessState = false;
                serviceResult.UserMsg = Properties.Resources.CrossLang_ResponseMessage_Duplicate_Email;
                return false;
            }

            serviceResult.SuccessState = true;
            return true && base.CustomValidate(entity, fields);
        }
    }
}

