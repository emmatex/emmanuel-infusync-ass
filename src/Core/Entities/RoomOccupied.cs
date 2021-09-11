using System;

namespace Core.Entities
{
    public class RoomOccupied
    {
        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public int RoomNumber { get; set; }
    }
}
