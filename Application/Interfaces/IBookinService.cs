using ZaminHotel.Domain.Entities;

namespace ZaminHotel.Application.Interfaces
{
    public interface IBookinService
    {
        Task<Booking> CreateBookingAsync(
            int userId,
            int roomId,
            DateTime CheckIn,
            DateTime CheckOut);

    }
}
