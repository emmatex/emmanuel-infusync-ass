using Core.Common;
using Core.Enums;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Core.Entities
{
    [BsonCollection("Room")]
    public class Room : Document
    {
        public int RoomNumber { get; set; }
        public string Description { get; set; }
        public RoomType RoomType { get; set; }
        public bool CheckIns { get; set; }
        public float Amount { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}
