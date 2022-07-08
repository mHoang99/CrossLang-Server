using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossLang.Library;

namespace CrossLang.ApplicationCore.Entities
{
    [TableName("user_lesson_status")]
    public class UserLessonStatus : BaseEntity
    {
        [DBColumn]
        [DisplayName("Nguời dùng")]
        public long? UserID { get; set; }

        [DBColumn]
        [DisplayName("Bài học")]
        public long? LessonID { get; set; }

        [DBColumn]
        [DisplayName("Đã hoàn thành")]
        public bool? IsFinished { get; set; }
    }
}
