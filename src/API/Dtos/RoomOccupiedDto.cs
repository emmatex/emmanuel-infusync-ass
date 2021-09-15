using System;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class RoomOccupiedDto
    {
        public string Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int RoomNumber { get; set; }
        public string FullName { get; set; }
        public bool IsOccupancy { get; set; }
        public float Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class RoomOccupiedManipulatedDto
    {
        [Required]
        public int RoomNumber { get; set; }
        [Required]
        public DateTime From { get; set; }
        [Required]
        public DateTime To { get; set; }
        [Required]
        [Range(0.1, float.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public float Amount { get; set; }
        [Required]
        public bool IsOccupancy { get; set; }

    }

    public class CreateRoomOccupiedDto : RoomOccupiedManipulatedDto
    {

    }

    public class UpdateRoomOccupiedDto : RoomOccupiedManipulatedDto
    {

    }
}
