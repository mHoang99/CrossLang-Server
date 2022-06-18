using CrossLang.ApplicationCore.Enums;
using CrossLang.Library;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossLang.ApplicationCore.Entities
{
    public class QuestionMongo
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string ID { get; set; }

        [DBColumn]
        [DisplayName("Nội dung câu hỏi")]
        public string? Content { get; set; }

        [DBColumn]
        [DisplayName("Các lựa chọn")]
        public List<string>? Options { get; set; }

        [DBColumn]
        [DisplayName("Câu trả lời")]
        public List<string>? Answers { get; set; }

        [DBColumn]
        [DisplayName("Loại câu hỏi")]
        public QuestionType? Type { get; set; }

        [DBColumn]
        [DisplayName("Bài tập liên quan")]
        public long? ExerciseID { get; set; }
    }
}
