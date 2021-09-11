using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Room
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public string Description { get; set; }

        public DateTime LastBooked { get; set; }

        public int Level { get; set; }

        public RoomType RoomType { get; set; }

        public float Amount { get; set; }
    }
}
