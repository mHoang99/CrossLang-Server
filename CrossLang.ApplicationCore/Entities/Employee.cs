using CrossLang.ApplicationCore.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossLang.ApplicationCore.Entities
{
    /// <summary>
    /// Model cho bảng nhân viên
    /// </summary>
    /// CREATED_BY: vmhoang
    [TableName("employee")]
    class Employee : BaseEntity
    {
        #region Properties
        [Required]
        [Unique]
        [MaxLength(20)]
        [DisplayName("Mã nhân viên")]
        public string? EmployeeCode { get; set; }

        [MaxLength(500)]
        [DisplayName("Ảnh chân dung")]
        public string? PersonalImage { get; set; }
        #endregion
    }
}
