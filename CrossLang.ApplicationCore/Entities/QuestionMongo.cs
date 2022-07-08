using System.Text.Json.Serialization;
using CrossLang.ApplicationCore.Enums;
using CrossLang.Library;
using MongoDB.Bson.Serialization.Attributes;

namespace CrossLang.ApplicationCore.Entities
{
    [CollectionName("Question")]
    public class QuestionMongo
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? ID { get; set; }

        public string? Content { get; set; }

        public List<string>? Options { get; set; }

        public List<string>? Answers { get; set; }

        public QuestionType? Type { get; set; }

        public long? ExerciseID { get; set; }
    }
}
