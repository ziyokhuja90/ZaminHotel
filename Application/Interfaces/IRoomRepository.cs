using ZaminHotel.Domain.Entities;

namespace ZaminHotel.Application.Interfaces
{
    public interface IRoomRepository
    {
        Task<Room?> GetByIdAsync(int id);

        Task<IEnumerable<Room>> GetAllAsync(int skip, int take);

        Task AddAsync(Room room);

        void Update(Room room);

        void Delete(Room room);

        Task<bool> ExistsAsync(int id);

        Task<int> SaveChangesAsync();
    }
}