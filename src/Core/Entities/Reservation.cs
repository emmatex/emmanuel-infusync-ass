using System;

namespace Core.Entities
{
    public class Reservation
    {
        public int Id { get; set; }

        public int RoomId { get; set; }

        public Room Room { get; set; }

      //  public List<Profile> Profiles { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }
}
