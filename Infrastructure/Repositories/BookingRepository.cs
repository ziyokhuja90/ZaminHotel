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
        
        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
        }

        public async Task<IEnumerable<Booking>> GetByUserIdAsync(int userId)
        {
            return await _context.Bookings
                .Include(b => b.Room)
                .Where(b => b.UserId == userId && !b.IsDeleted)
                .OrderByDescending(b => b.Id)
                .ToListAsync();
        }
        public async Task<IEnumerable<Booking>> GetAllAsync(int skip, int take)
        {
            return await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .Where(b => !b.IsDeleted)
                .OrderByDescending(b => b.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }
        public async Task AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
        }

        public void Update(Booking booking)
        {
            _context.Bookings.Update(booking);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}