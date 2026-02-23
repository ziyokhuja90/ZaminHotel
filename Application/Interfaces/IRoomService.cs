using ZaminHotel.Domain.Entities;

namespace ZaminHotel.Application.Interfaces
{
    public interface IRoomService
    {
        Task<Room?> GetByIdAsync(int id);

        Task<IEnumerable<Room>> GetAllAsync(int page, int pageSize);

        Task CreateAsync(Room room);

        Task UpdateAsync(int id, Room updatedRoom);

        Task DeleteAsync(int id);
    }
}