using ZaminHotel.Domain.Entities;

namespace ZaminHotel.Application.Interfaces
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetOverlappingBookingAsync(
            int roomId,
            DateTime checkIn,
            DateTime checkOut);

        Task AddAsync(Booking booking);

        Task<int> SaveChangesAsync();
    }
}   