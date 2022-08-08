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
        [DisplayColumn]
        [DisplayName("Tiêu đề bài giảng")]
        public string? Title { get; set; }

        [DBColumn]
        [DisplayName("Mô tả bài giảng")]
        public string? Description { get; set; }

        [DBColumn]
        [DisplayColumn]
        [DisplayName("Link video")]
        public string? VideoLinks { get; set; }

        [DBColumn]
        [DisplayColumn]
        [DisplayName("Cấp độ")]
        public LessonLevelEnum? Level { get; set; }

        [DBColumn]
        [DisplayColumn]
        [DisplayName("Link tài liệu")]
        public string? DocumentLinks { get; set; }

        [DisplayName("Nội dung HTML")]
        public string? Content { get; set; }

        [DBColumn]
        [DisplayName("Phân loại")]
        [DisplayColumn]
        public long? LessonCategoryID { get; set; }

        [DBColumn]
        [DisplayName("Thumbnail")]
        public string? Thumbnail { get; set; }

        [DBColumn]
        [DisplayName("Người viết bài")]
        public long? UserID { get; set; }

        [DBColumn]
        [DisplayColumn]
        [DisplayName("Số người học")]
        public long? LearnerCount { get; set; }

        public List<string>? DictionaryWords { get; set; }

        public List<long>? DictionaryWordIDs { get; set; }

        public long? FlashCardCollectionID { get; set; }

        public bool? IsFinished { get; set; }

        public long? LearnerID { get; set; }

        [DisplayColumn]
        [DisplayName("Tài khoản tạo")]
        public string? CreatorUsername { get; set; }

        [DisplayColumn]
        [DisplayName("Người tạo")]
        public string? CreatorFullname { get; set; }

        [DisplayColumn]
        [DisplayName("Vai trò người tạo")]
        public RoleEnum? CreatorRoleID { get; set; }


        [DisplayColumn]
        [DisplayName("Vai trò người tạo")]
        public RoleEnum? CreatorRoleName { get; set; }

        [DisplayColumn]
        [DisplayName("Chủ đề")]
        public string? CategoryName { get; set; }

        [DisplayColumn]
        [DisplayName("Ảnh chủ đề")]
        public string? CatgegoryImage { get; set; }

        [DisplayColumn]
        [DisplayName("Cấp độ")]
        public string? LevelName
        {
            get
            {
                if (Level == null)
                {
                    return "";
                }
                return Enum.GetName(typeof(LessonLevelEnum), this.Level);
            }
        }

        [DBColumn]
        [DisplayColumn]
        [DisplayName("Gói")]
        public int Package { get; set; }

    }
}
