using CrossLang.ApplicationCore.Enums;
using CrossLang.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossLang.ApplicationCore.Entities
{
    [TableName("exercise")]
    public class Exercise : BaseEntity
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


        [DBColumn]
        [DisplayName("Có trộn")]
        public long? LessonID { get; set; }

        public List<QuestionMongo>? Questions { get; set; }
    }
}
