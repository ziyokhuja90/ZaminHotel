using ZaminHotel.Domain.Entities;
using ZaminHotel.Application.DTOs;

namespace ZaminHotel.Application.Interfaces
{
    public interface IBookingService
    {
        Task<Booking> CreateBookingAsync(
            int userId,
            int roomId,
            DateTime CheckIn,
            DateTime CheckOut);

        Task<IEnumerable<BookingResponseDto>> GetMyBookingsAsync(int userId);

        Task<IEnumerable<BookingResponseDto>> GetAllBookingsAsync(int page, int pageSize);

        Task CancelBookingAsync(int bookingId, int userId);
    }
}
