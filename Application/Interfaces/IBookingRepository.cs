using ZaminHotel.Domain.Entities;

namespace ZaminHotel.Application.Interfaces
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetOverlappingBookingAsync(
            int roomId,
            DateTime checkIn,
            DateTime checkOut);
        Task<Booking?> GetByIdAsync(int id);

        Task<IEnumerable<Booking>> GetByUserIdAsync(int userId);
        
        Task<IEnumerable<Booking>> GetAllAsync(int skip, int take);

        Task AddAsync(Booking booking);

        void Update(Booking booking);

        Task<int> SaveChangesAsync();
    }
}   