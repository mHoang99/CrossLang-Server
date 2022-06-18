using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossLang.Library;

namespace CrossLang.ApplicationCore.Entities
{
    [TableName("dictionary_word")]
    public class DictionaryWord : BaseEntity
    {
        [DisplayName("Text của từ")]
        [MaxLength(255)]
        [DBColumn]
        public string? Text { get; set; }

        [DisplayName("Phát âm")]
        [MaxLength(255)]
        [DBColumn]
        public string? Pronunciation { get; set; }

        [DisplayName("Ảnh minh họa")]
        [MaxLength(500)]
        [DBColumn]
        public string? Image { get; set; }


        public long? FlashCardID { get; set; }

        public long? FlashCardCollectionID { get; set; }
    }
}
