using System;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Enums;
using CrossLang.Models;

namespace CrossLang.ApplicationCore.Interfaces.IService
{
    public interface IUpgradeCodeService : IBaseService<UpgradeCode>
    {
        ServiceResult GenerateCode(PackageEnum package, long? period, bool? isTrial, long? price);
        ServiceResult RedeemCode(string? code);
    }
}

