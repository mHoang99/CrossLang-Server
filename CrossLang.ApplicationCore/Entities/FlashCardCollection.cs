using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossLang.Library;

namespace CrossLang.ApplicationCore.Entities
{
    [TableName("flash_card_collection")]
    public class FlashCardCollection : BaseEntity
    {
        [DBColumn]
        [DisplayName("Chủ sở hữu")]
        public long? UserID { get; set; }

        [DBColumn]
        [DisplayName("Tiêu đề")]
        [MaxLength(255)]
        public string? Title { get; set; }

        [DBColumn]
        [DisplayName("Mô tả")]
        public string? Description { get; set; }

        [DBColumn]
        [DisplayName("Công khai")]
        public bool? IsPublic { get; set; }

        [DBColumn]
        [DisplayName("Certified")]
        public bool? IsCertified { get; set; }

        [DBColumn]
        [DisplayName("Đánh giá")]
        public float? Rating { get; set; }

        [DBColumn]
        [DisplayName("Bài học liên quan")]
        public long? LessonID { get; set; }


        [DBColumn]
        [DisplayName("Số từ")]
        public int? WordCount { get; set; }

        public int Progress { get; set; }
    }
}
