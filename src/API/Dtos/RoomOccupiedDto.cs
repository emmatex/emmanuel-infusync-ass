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
        public string Email { get; set; }
        public double TotalAmount { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class RoomOccupiedManipulatedDto
    {
        [Required]
        public int RoomNumber { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public DateTime From { get; set; }
        [Required]
        public DateTime To { get; set; }
    }

    public class CreateRoomOccupiedDto : RoomOccupiedManipulatedDto
    {

    }

    public class UpdateRoomOccupiedDto : RoomOccupiedManipulatedDto
    {

    }
}
