using Microsoft.EntityFrameworkCore;
using ZaminHotel.Application.Interfaces;
using ZaminHotel.Domain.Entities;
using ZaminHotel.Infrastructure.Data;

namespace ZaminHotel.Infrastructure.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly AppDbContext _context;

        public RoomRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Room?> GetByIdAsync(int id)
        {
            return await _context.Rooms
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
        }

        public async Task<IEnumerable<Room>> GetAllAsync(int skip, int take)
        {
            return await _context.Rooms
                .Where(r => !r.IsDeleted)
                .OrderBy(r => r.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task AddAsync(Room room)
        {
            await _context.Rooms.AddAsync(room);
        }

        public void Update(Room room)
        {
            _context.Rooms.Update(room);
        }

        public void Delete(Room room)
        {
            room.IsDeleted = true;
            _context.Rooms.Update(room);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Rooms
                .AnyAsync(r => r.Id == id && !r.IsDeleted);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}