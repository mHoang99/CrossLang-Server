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
        [DisplayColumn]
        [DisplayName("Tên")]
        public string? Name { get; set; }

        [DBColumn]
        [DisplayColumn]
        [DisplayName("Mô tả")]
        public string? Description { get; set; }

        [DBColumn]
        [DisplayColumn]
        [DisplayName("Loại")]
        public ExerciseType? Type { get; set; }

        [DBColumn]
        [DisplayColumn]
        [DisplayName("Có trộn")]
        public bool? IsShuffle { get; set; }


        [DBColumn]
        [DisplayColumn]
        [DisplayName("ID Bài học")]
        public long? LessonID { get; set; }

        public List<QuestionMongo>? Questions { get; set; }

        [DisplayColumn]
        [DisplayName("Số câu hỏi")]
        public int? QuestionCount
        {
            get
            {
                return Questions?.Count;
            }
        }

        [DisplayColumn]
        [DisplayName("Bài học")]
        public string? LessonTitle { get; set; }

        [DisplayColumn]
        [DisplayName("Cấp độ")]
        public LessonLevelEnum? LessonLevel { get; set; }

        [DisplayColumn]
        [DisplayName("ID Người tạo")]
        public long? LessonCreatorID { get; set; }

        [DisplayColumn]
        [DisplayName("Cấp độ")]
        public string? LessonLevelName
        {
            get
            {
                if (LessonLevel == null)
                {
                    return "";
                }
                return Enum.GetName(typeof(LessonLevelEnum), this.LessonLevel);
            }
        }

    }
}
