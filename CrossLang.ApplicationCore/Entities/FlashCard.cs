using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossLang.ApplicationCore.Entities
{
    [TableName("flash_card")]
    public class FlashCard : BaseEntity
    {
        [DBColumn]
        [DisplayName("Nguời dùng")]
        public long? UserID { get; set; }

        [DBColumn]
        [DisplayName("Từ vựng")]
        public long? DictionaryWordID { get; set; }

        [DBColumn]
        [DisplayName("Tập flash card")]
        public long? FlashCardCollectionID { get; set; }
    }
}
