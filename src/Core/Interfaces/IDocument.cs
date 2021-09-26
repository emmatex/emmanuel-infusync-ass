using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Core.Interfaces
{
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        ObjectId Id { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        DateTime CreatedOn { get; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        DateTime UpdatedDate { get; set; }
    }
}
