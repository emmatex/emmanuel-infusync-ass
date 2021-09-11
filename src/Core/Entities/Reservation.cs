using Core.Common;
using System;

namespace Core.Entities
{
    [BsonCollection("Reservation")]
    public class Reservation : Document
    {
        public int RoomNumber { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Email { get; set; }
    }
}
