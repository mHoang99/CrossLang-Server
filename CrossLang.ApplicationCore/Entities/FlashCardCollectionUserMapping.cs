using System;
using System.ComponentModel;
using CrossLang.Library;

namespace CrossLang.ApplicationCore.Entities
{
    [TableName("flash_card_collection_user_mapping")]
    public class FlashCardCollectionUserMapping : BaseEntity
    {
        [DBColumn]
        [DisplayName("User lien quan")]
        public long? UserID { get; set; }

        [DBColumn]
        [DisplayName("Collection lien quan")]
        public long? FlashCardCollectionID { get; set; }
        [DBColumn]
        [DisplayName("Đang theo dõi")]
        public Boolean? IsFollowing { get; set; }

        [DBColumn]
        [DisplayName("Đánh giá")]
        public float? Rating { get; set; }

        [DBColumn]
        [DisplayName("Quá trình")]
        public int? Progress { get; set; }
    }
}

