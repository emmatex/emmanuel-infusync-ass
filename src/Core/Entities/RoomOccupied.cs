using Core.Common;
using System;

namespace Core.Entities
{
    [BsonCollection("RoomOccupied")]
    public class RoomOccupied : Document
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int RoomNumber { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public bool IsOccupancy { get; set; }
        public float Amount { get; set; }
    }
}
