using CrossLang.ApplicationCore.Enums;
using CrossLang.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CrossLang.Library;

namespace CrossLang.ApplicationCore.Entities
{
    /// <summary>
    /// Model cho bảng user
    /// </summary>
    /// CREATED_BY: vmhoang
    [TableName("user")]
    public class User : BaseEntity
    {
        #region Properties
        [Required]
        [Unique]
        [MaxLength(50)]
        [DBColumn]
        [DisplayColumn]
        [DisplayName("Tên tài khoản")]
        public string? Username { get; set; }

        [JsonIgnore]
        [Required]
        [DBColumn]
        [DisplayName("Mật khẩu")]
        public string? Password { get; set; }

        [DBColumn]
        [MaxLength(255)]
        [DisplayName("Hình đại diện")]
        public string? Avatar { get; set; }

        [DBColumn]
        [MaxLength(100)]
        [DisplayName("Họ và tên")]
        [DisplayColumn]

        public string? FullName { get; set; }

        [DBColumn]
        [DisplayName("Ngày sinh")]
        [DisplayColumn]

        public DateTime? DateOfBirth { get; set; }

        [DBColumn]
        [MaxLength(255)]
        [DisplayName("Email")]
        [DisplayColumn]

        public string? Email { get; set; }

        [DBColumn]
        [MaxLength(255)]
        [DisplayColumn]
        [DisplayName("Giới tính")]

        public Gender? Gender { get; set; }

        [DBColumn]
        [DisplayColumn]
        [MaxLength(255)]
        [DisplayName("Số điện thoại")]
        public string? PhoneNumber { get; set; }

        [DBColumn]
        [DisplayColumn]
        [DisplayName("Địa chỉ")]
        public string? Address { get; set; }

        [DBColumn]
        [DisplayColumn]
        [DisplayName("Vai trò")]
        public long? RoleID { get; set; }

        [DBColumn]
        [DisplayColumn]
        [DisplayName("Quyền của người dùng")]
        public long? UserPermission { get; set; }

        [DBColumn]
        [DisplayColumn]
        [DisplayName("Quyền của gói")]
        public long? PackagePermission { get; set; }

        [DBColumn]
        [Unique]
        [DisplayName("Nhân viên tương ứng")]
        public long? EmployeeId { get; set; }

        public string? RegisterPassword { get; set; }
        #endregion
    }
}
