using CrossLang.ApplicationCore.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossLang.ApplicationCore.Entities
{
    abstract class DictionaryWordTranslate : BaseEntity
    {
        [DBColumn]
        [DisplayName("Từ vựng")]
        public long? DictionaryWordID { get; set; }

        [DBColumn]
        [DisplayName("Dịch nghĩa")]
        public string? Translation { get; set; }

        [DBColumn]
        [DisplayName("Loại từ")]
        public WordType? WordType { get; set; }

        [DBColumn]
        [DBJSON]
        [DisplayName("Ví dụ")]

        public string? Examples { get; set; }
    }
}
