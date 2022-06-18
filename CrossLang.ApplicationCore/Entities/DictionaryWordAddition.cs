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
    abstract class DictionaryWordAddition : BaseEntity
    {
        [DBColumn]
        [DisplayName("Từ vựng")]
        public long? DictionaryWordID { get; set; }

        [DBColumn]
        [DisplayName("Loại từ")]
        public WordType? WordType { get; set; }

        [DBColumn]
        [DBJSON]
        [DisplayName("Nội dung")]

        public string? JsonContent { get; set; }
    }
}
