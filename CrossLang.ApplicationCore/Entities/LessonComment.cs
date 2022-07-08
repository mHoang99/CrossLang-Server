using System;
using System.ComponentModel;
using CrossLang.Library;

namespace CrossLang.ApplicationCore.Entities
{
    [TableName("lesson_comment")]
    public class LessonComment : BaseEntity
    {
        [DisplayName("ID Người dùng")]
        [DBColumn]
        public long? UserID { get; set; }

        [DisplayName("ID bài giảng")]
        [DBColumn]
        public long? LessonID { get; set; }

        [DBColumn]
        [DisplayName("Nội dung comment")]
        public string? Content { get; set; }

        [DBColumn]
        [DisplayName("Trả lời comment")]
        public long? ReplyToID { get; set; }
    }
}

