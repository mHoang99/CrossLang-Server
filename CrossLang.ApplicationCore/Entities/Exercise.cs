using CrossLang.ApplicationCore.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossLang.ApplicationCore.Entities
{
    [TableName("exercise")]
    class Exercise : BaseEntity
    {
        [DBColumn]
        [DisplayName("Tên")]
        public string? Name { get; set; }
        
        [DBColumn]
        [DisplayName("Mô tả")]
        public string? Description { get; set; }

        [DBColumn]
        [DisplayName("Loại")]
        public ExerciseType? Type { get; set; }

        [DBColumn]
        [DisplayName("Có trộn")]
        public bool? IsShuffle { get; set; }
    }
}
