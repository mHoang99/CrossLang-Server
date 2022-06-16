using CrossLang.ApplicationCore.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossLang.ApplicationCore.Entities
{
    [TableName("dictionary_word_related_mapping")]
    class DictionaryWordRelatedMapping : BaseEntity
    {
        [DBColumn]
        [DisplayName("ID của từ")]
        public long? DictionaryWordID { get; set; }

        [DBColumn]
        [MaxLength(255)]
        [DisplayName("Text của từ")]
        public string? DictionaryWordText { get; set; }

        [DBColumn]
        [DisplayName("Text của từ")]
        public string? Context { get; set; }

        [DBColumn]
        [DisplayName("Text của từ")]
        public string? RelatedWords { get; set; }

        [DBColumn]
        [DisplayName("Loại từ")]
        public WordType? WordType { get; set; }

        [DBColumn]
        [DisplayName("Từ đồng nghĩa, trái nghĩa")]
        public DictionaryWordMappingType? Type { get; set; }


    }
}
