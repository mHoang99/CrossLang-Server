using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossLang.Library;

namespace CrossLang.ApplicationCore.Entities
{
    [TableName("lesson_category")]
    public class LessonCategory : BaseEntity
    {
        [DBColumn]
        [MaxLength(255)]
        [DisplayName("Tên")]
        public string? Name { get; set; }

        [DBColumn]
        [DisplayName("Mô tả")]
        public string? Description { get; set; }
        [DBColumn]
        [DisplayName("Quyền truy cập theo gói")]
        public long? PackagePermission { get; set; }
        [DBColumn]
        [MaxLength(500)]
        [DisplayName("Hình ảnh")]
        public string? Image { get; set; }
    }
}
