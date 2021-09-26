using Core.Common;
using Core.Enums;
using System;

namespace Core.Entities
{
    [BsonCollection("Room")]
    public class Room : Document
    {
        public int RoomNumber { get; set; }
        public string Description { get; set; }
        public RoomType RoomType { get; set; }
        public double RoomAmount { get; set; } // amount per night for a particular room 
        public double TotalAmount { get; set; } // amount * number of days
        public RoomState RoomState { get; set; }
        public string FullName { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Email { get; set; }
        public ClientState ClientState { get; set; }
    }
}
