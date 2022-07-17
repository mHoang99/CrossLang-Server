using System;
using CrossLang.Library;
using MongoDB.Bson.Serialization.Attributes;

namespace CrossLang.ApplicationCore.Entities
{
    [CollectionName("Lesson")]
	public class LessonContentMongo
	{
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string ID { get; set; }

        public long LessonID { get; set; }

        public string? LessonContent { get; set; }
    }
}

