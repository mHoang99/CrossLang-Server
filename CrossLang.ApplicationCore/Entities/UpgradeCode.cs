using System;
using System.ComponentModel;
using CrossLang.ApplicationCore.Enums;
using CrossLang.Library;

namespace CrossLang.ApplicationCore.Entities
{
    [TableName("upgrade_code")]
    public class UpgradeCode : BaseEntity
    {
        [DBColumn]
        [DisplayColumn]
        [DisplayName("Mã")]
        public string Code { get; set; }

        [DBColumn]
        [DisplayColumn]
        [DisplayName("Ngày mã hết hạn")]
        public DateTime ValidUntil { get; set; }

        [DBColumn]
        [DisplayColumn]
        [DisplayName("Giá")]
        public long? Price { get; set; }

        [DBColumn]
        [DisplayColumn]
        [DisplayName("Thời hạn")]
        public long? Period { get; set; }

        [DBColumn]
        [DisplayColumn]
        [DisplayName("Gói")]
        public PackageEnum Package { get; set; }

        [DBColumn]
        [DisplayColumn]
        [DisplayName("Là dùng thử")]
        public bool? IsTrial { get; set; }
    }
}

