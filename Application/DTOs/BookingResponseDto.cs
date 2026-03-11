namespace ZaminHotel.Application.DTOs
{
    public class BookingResponseDto
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; } = null!;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = null!;
    }
}
