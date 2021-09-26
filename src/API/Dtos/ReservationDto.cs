using System;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class ReservationDto
    {
        public string Id { get; set; }
        public int RoomNumber { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Email { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class CreateReservationDto
    {
        [Required]
        public int RoomNumber { get; set; }
        [Required]
        public DateTime From { get; set; }
        [Required]
        public DateTime To { get; set; }
    }
}
