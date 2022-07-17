using System;
using CrossLang.ApplicationCore.Enums;

namespace CrossLang.API.Models.Requests
{
    public class GenerateUpgradeCodeRequest
    {
        public long? Period { get; set; }

        public PackageEnum Package { get; set; }

        public bool? IsTrial { get; set; }

        public long? Price { get; set; }
    }

    public class RedeemUpgradeCodeRequest
    {
        public string? Code { get; set; }
    }
}

