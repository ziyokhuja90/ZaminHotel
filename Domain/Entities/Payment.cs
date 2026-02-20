using ZaminHotel.Domain.Common;

namespace ZaminHotel.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public int BookingId { get; set; }
        public Booking Booking { get; set; } = null!;

        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; } = "Unpaid";
    }
}