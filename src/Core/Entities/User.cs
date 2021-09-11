using Core.Common;
using Core.Enums;

namespace Core.Entities
{
    [BsonCollection("User")]
    public class User : Document
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string PersonalID { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public UserRole Role { get; set; }
    }
}
