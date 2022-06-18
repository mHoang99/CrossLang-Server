using CrossLang.ApplicationCore.Entities;
using CrossLang.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossLang.ApplicationCore.Entities
{
    /// <summary>
    /// Model cho bảng RefreshToken
    /// </summary>
    /// CREATED_BY: vmhoang
    [TableName("refresh_token")]
    public class RefreshToken : BaseEntity
    {
        #region Properties
        
        [DBColumn]
        [DisplayName("Giá trị")]
        public string? HashedValue { get; set; }

        [DBColumn]
        [DisplayName("Người dùng")]
        public long UserId { get; set; }
        #endregion
    }
}
