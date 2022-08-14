using System;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Enums;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Interfaces.IRepository;
using CrossLang.ApplicationCore.Interfaces.IService;
using CrossLang.Library;
using CrossLang.Models;
using CrossLang.QueueHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CrossLang.ApplicationCore.Services
{
    public class UpgradeCodeService : BaseService<UpgradeCode>, IUpgradeCodeService
    {
        private IConfiguration _configuration;

        private IUserRepository _userRepository;

        private IRedeemHistoryRepository _redeemHistoryRepository;


        public UpgradeCodeService(IUpgradeCodeRepository repository, IHttpContextAccessor httpContextAccessor, SessionData sessionData, IConfiguration configuration, IUserRepository userRepository, IRedeemHistoryRepository redeemHistoryRepository) : base(repository, httpContextAccessor, sessionData)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _redeemHistoryRepository = redeemHistoryRepository;
        }


        protected override void AfterAdd(ref UpgradeCode entity)
        {
            base.AfterAdd(ref entity);

            SendEmailToUser(entity.Code);
        }

        private void SendEmailToUser(string code)
        {
            var emailQueue = _configuration["RabbitMQ:EmailQueue"];

            RabbitMQMessage<EmailMessage> message = new RabbitMQMessage<EmailMessage>
            {
                Body = new EmailMessage
                {
                    To = _sessionData.Email,
                    Subject = "[CrossLang] Nâng cấp tài khoản",
                    Body = $"<div>{code}</div>"
                },
                UserID = _sessionData.ID
            };

            RabbitMQHelper.Enqueue(emailQueue, message);
        }

        public ServiceResult RedeemCode(string? code)
        {
            var upgradeCode = _repository.GetEntityByColumns(new UpgradeCode { Code = code ?? "" }, new List<string> { "Code" });

            var currentUser = _userRepository.GetEntityById(_sessionData.ID);


            if (upgradeCode != null)
            {
                var expDate = DateTime.Now.AddDays(upgradeCode.Period ?? 0);

                if (upgradeCode.Package == currentUser.Package)
                {
                    expDate = currentUser.ExpDate?.AddDays(upgradeCode.Period ?? 0) ?? expDate;
                }


                var entity = new RedeemHistory
                {
                    EntityState = EntityState.ADD,

                    Package = upgradeCode.Package,

                    Period = upgradeCode.Period,

                    Price = upgradeCode.Price,

                    ExpDate = expDate,

                    UserID = _sessionData.ID,

                    CreatedDate = DateTime.Now,
                    CreatedBy = _sessionData.Username,

                    ModifiedDate = DateTime.Now,
                    ModifiedBy = _sessionData.Username,
                };

                _redeemHistoryRepository.Add(entity);

                var userEntity = new User
                {
                    ID = _sessionData.ID,
                    Package = entity.Package,
                    ExpDate = expDate
                };

                var userUpdateFields = new List<string>
                {
                    "Package", "ExpDate"
                };

                if (upgradeCode.IsTrial ?? false)
                {
                    userEntity.IsTrialUsed = true;
                    userUpdateFields.Append("IsTrialUsed");
                }

                _userRepository.UpdateFields(userUpdateFields, userEntity);

                Delete(upgradeCode.ID);

                serviceResult.SuccessState = true;
                serviceResult.Data = new
                {
                    UpgradeCode = upgradeCode,
                    Package = upgradeCode.Package,
                    ExpDate = expDate
                };
                return serviceResult;
            }

            else
            {
                serviceResult.SuccessState = false;
                serviceResult.UserMsg = "Mã không tồn tại";
                serviceResult.DevMsg = "Mã không tồn tại";
                return serviceResult;
            }
        }

        public ServiceResult ValidateCode(string? code)
        {
            var upgradeCode = _repository.GetEntityByColumns(new UpgradeCode { Code = code ?? "" }, new List<string> { "Code" });

            if (upgradeCode != null)
            {
                serviceResult.SuccessState = true;
                serviceResult.Data = new
                {
                    Package = upgradeCode.Package,
                    Period = upgradeCode.Period,
                    ValidUntil = upgradeCode.ValidUntil,
                };
                return serviceResult;
            }

            else
            {
                serviceResult.SuccessState = false;
                serviceResult.UserMsg = "Mã không tồn tại";
                serviceResult.DevMsg = "Mã không tồn tại";
                return serviceResult;
            }
        }


        public ServiceResult GenerateCode(PackageEnum package, long? period, bool? isTrial, long? price)
        {
            var newCode = CLHelper.RandomString(12);

            var expDate = DateTime.Now.AddMonths(24);

            var code = new UpgradeCode
            {
                EntityState = EntityState.ADD,

                Package = package,

                Period = period,

                IsTrial = isTrial,

                Price = price,

                ValidUntil = expDate,

                Code = newCode
            };

            return Add(code);
        }
    }
}

