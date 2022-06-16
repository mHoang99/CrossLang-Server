using CrossLang.ApplicationCore.Enums;
using CrossLang.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
        public string? FullName { get; set; }

        [DBColumn]
        [DisplayName("Ngày sinh")]
        public DateTime? DateOfBirth { get; set; }

        [DBColumn]
        [MaxLength(255)]
        [DisplayName("Email")]
        public string? Email { get; set; }

        [DBColumn]
        [MaxLength(255)]
        [DisplayName("Giới tính")]
        public Gender? Gender { get; set; }

        [DBColumn]
        [MaxLength(255)]
        [DisplayName("Số điện thoại")]
        public string? PhoneNumber { get; set; }

        [DBColumn]
        [DisplayName("Địa chỉ")]
        public string? Address { get; set; }

        [DBColumn]
        [DisplayName("Vai trò")]
        public long? RoleID { get; set; }

        [DBColumn]
        [DisplayName("Quyền của người dùng")]
        public long? UserPermission { get; set; }

        [DBColumn]
        [DisplayName("Quyền của gói")]
        public long? PackagePermission { get; set; }

        [DBColumn]
        [Unique]
        [DisplayName("Nhân viên tương ứng")]
        public long? EmployeeId { get; set; }
        #endregion
    }
}
