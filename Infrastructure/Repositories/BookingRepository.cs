using Microsoft.EntityFrameworkCore;
using ZaminHotel.Application.Interfaces;
using ZaminHotel.Domain.Entities;
using ZaminHotel.Infrastructure.Data;

namespace ZaminHotel.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;

        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Booking>> GetOverlappingBookingAsync(
            int roomId,
            DateTime checkIn,
            DateTime checkOut)
        {
            return await _context.Bookings
                .Where(b =>
                    b.RoomId == roomId &&
                    b.Status != "Cancelled" &&
                    b.CheckInDate < checkOut &&
                    b.CheckOutDate > checkIn)
                .ToListAsync();
        }

        public async Task AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}