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
    internal class Question : BaseEntity
    {
        [DBColumn]
        [DisplayName("Nội dung câu hỏi")]
        public string? Content { get; set; }

        [DBColumn]
        [DisplayName("Các lựa chọn")]
        public string? Options { get; set; }

        [DBColumn]
        [DisplayName("Câu trả lời")]
        public string? Answers { get; set; }

        [DBColumn]
        [DisplayName("Loại câu hỏi")]
        public QuestionType? Type { get; set; }
    }
}
