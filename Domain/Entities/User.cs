using ZaminHotel.Domain.Common;

namespace ZaminHotel.Domain.Entities
{
    public class User: BaseEntity
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Role { get; set; } = "Customer"; // Default role is Customer
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
