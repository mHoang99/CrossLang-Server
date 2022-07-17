using System;
using System.ComponentModel;
using CrossLang.ApplicationCore.Enums;
using CrossLang.Library;

namespace CrossLang.ApplicationCore.Entities
{
    [TableName("redeem_history")]
    public class RedeemHistory : BaseEntity
    {
        [DBColumn]
        [DisplayColumn]
        [DisplayName("Người dùng")]
        public long? UserID { get; set; }

        [DBColumn]
        [DisplayColumn]
        [DisplayName("Ngày hết hạn")]
        public DateTime? ExpDate { get; set; }

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
    }
}

