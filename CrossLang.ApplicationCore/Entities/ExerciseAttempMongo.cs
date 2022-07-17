using System;
using System.ComponentModel;
using CrossLang.Library;
using MongoDB.Bson.Serialization.Attributes;

namespace CrossLang.ApplicationCore.Entities
{
    [CollectionName("ExerciseAttemp")]
    public class ExerciseAttempMongo
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? ID { get; set; }

        public long? UserID { get; set; }

        [DisplayColumn]
        [DisplayName("Câu trả lời")]
        public List<ExerciseAttempQuestion>? AttempAnswers { get; set; }

        [DisplayColumn]
        [DisplayName("Câu trả đúng")]
        public int? CorrectAnswerCount { get; set; }

        [DisplayColumn]
        [DisplayName("Số câu trả lời")]
        public int? TotalAnswerCount { get; set; }

        [DisplayColumn]
        [DisplayName("ID Bài tập")]
        public long? ExerciseID { get; set; }

        [DisplayColumn]
        [DisplayName("ID Bài giảng")]
        public long? LessonID { get; set; }

        [DisplayColumn]
        [DisplayName("Ngày làm bài")]
        public DateTime? CreatedDate { get; set; }

        [DisplayColumn]
        [DisplayName("Tên đăng nhập")]
        public string? Username { get; set; }

        [DisplayColumn]
        [DisplayName("Họ và tên")]
        public string? Fullname { get; set; }

        public int? Score { get; set; }
    }

    public class ExerciseAttempQuestion
    {
        public string QuestionID { get; set; }
        public List<string> Answers { get; set; }

    }
}

