using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossLang.ApplicationCore.Entities
{
    [TableName("user_lesson_status")]
    class UserExerciseStatus : BaseEntity
    {
        [DBColumn]
        [DisplayName("Người dùng")]
        public long? UserID { get; set; }

        [DBColumn]
        [DisplayName("Bài tập")]
        public long? ExerciseID { get; set; }

        [DBColumn]
        [DisplayName("Thời gian bắt đầu")]
        public DateTime? StartTime { get; set; }

        [DBColumn]
        [DisplayName("Thời gian kết thúc")]
        public DateTime? EndTime { get; set; }

        [DBColumn]
        [DisplayName("Số câu trả lời đúng")]
        public int? CorrectAnswers { get; set; }

        [DBColumn]
        [DisplayName("Tổng số câu trả lời")]
        public int? TotalAnswers { get; set; }
    }
}
