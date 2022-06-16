using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossLang.ApplicationCore.Entities
{
    [TableName("lesson_dictionary_word_mapping")]
    class LessonDictionaryWordMapping : BaseEntity
    {
        [DBColumn]
        [DisplayName("ID bài giảng")]
        public long? LessonID { get; set; }

        [DBColumn]
        [DisplayName("ID của từ")]
        public long? DictionaryWordID { get; set; }

        [DBColumn]
        [MaxLength(255)]
        [DisplayName("Text của từ")]
        public string? DictionaryWordText { get; set; }
    }
}
