using Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class RoomDto
    {
        public string Id { get; set; }
        public int RoomNumber { get; set; }
        public string Description { get; set; }
        public string RoomType { get; set; }
        public double RoomAmount { get; set; }
        public string RoomState { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class RoomManipulatedDto
    {
        [Required]
        public int RoomNumber { get; set; }
        [MaxLength(250, ErrorMessage = "The description shouldn't have more than 250 letters")]
        public virtual string Description { get; set; }
        [Required]
        public RoomType RoomType { get; set; }
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public double RoomAmount { get; set; }
    }

    public class CreateRoomDto : RoomManipulatedDto
    {
        
    }

    public class UpdateRoomDto : RoomManipulatedDto
    {
        [Required]
        public override string Description { get => base.Description; set => base.Description = value; }
    }

    public class FreeRoomDto
    {
        public string Id { get; set; }
        public int RoomNumber { get; set; }
    }
}
