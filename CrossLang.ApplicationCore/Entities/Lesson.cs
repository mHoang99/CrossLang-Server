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
    [TableName("lesson")]
    public class Lesson: BaseEntity
    {
        [DBColumn]
        [MaxLength(255)]
        [DisplayName("Tiêu đề bài giảng")]
        public string? Title { get; set; }

        [DBColumn]
        [DisplayName("Mô tả bài giảng")]
        public string? Description { get; set; }

        [DBColumn]
        [DisplayName("Link video bài giảng")]
        public string? VideoLinks { get; set; }

        [DBColumn]
        [DisplayName("Cấp độ của bài giảng")]
        public LessonLevel? Level { get; set; }

        [DBColumn]
        [DisplayName("Link tài liệu")]
        public string? DocumentLinks { get; set; }

        [DisplayName("Nội dung HTML")]
        public string? Content { get; set; }

        [DBColumn]
        [DisplayName("Phân loại")]
        public long? LessonCategoryID { get; set; }

        [DBColumn]
        [DisplayName("Thumbnail")]
        public string? Thumbnail { get; set; }

        public List<string>? DictionaryWords { get; set; }

        public List<long>? DictionaryWordIDs { get; set; }
    }
}
