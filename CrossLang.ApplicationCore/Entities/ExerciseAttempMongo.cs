using System;
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

        public List<ExerciseAttempQuestion>? AttempAnswers { get; set; }

        public int? CorrectAnswerCount { get; set; }


        public int? TotalAnswerCount { get; set; }

        public long? ExerciseID { get; set; }

        public long? LessonID { get; set; }

        public DateTime? CreatedDate;
    }

    public class ExerciseAttempQuestion
    {
        public string QuestionID { get; set; }
        public List<string> Answers { get; set; }

    }
}

