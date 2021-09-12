using Core.Enums;

namespace API.Dtos
{
    public class UserDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string PersonalID { get; set; }
        public UserRole Role { get; set; }
    }
}