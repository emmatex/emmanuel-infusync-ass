using Core.Enums;

namespace API.Dtos
{
    public class UserDto
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string PersonalID { get; set; }
    }

    public class CustomerReportDto
    {
        public string FullName { get; set; }
        public decimal Amount { get; set; }
        public int Days { get; set; }
    }
}