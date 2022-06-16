using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossLang.ApplicationCore.Entities
{
    internal class Role : BaseEntity
    {
        [DBColumn]
        [MaxLength(50)]
        [DisplayName("Tên vai trò")]
        public string? RoleName { get; set; }

        [DBColumn]
        [MaxLength(50)]
        [DisplayName("Mã vai trò")]
        public string? RoleCode{ get; set; }

        [DBColumn]
        [DisplayName("Quyền vai trò")]
        public long? RolePermission { get; set; }

    }
}
