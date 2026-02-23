using ZaminHotel.Application.Interfaces;
using ZaminHotel.Domain.Entities;

namespace ZaminHotel.Application.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<Room?> GetByIdAsync(int id)
        {
            return await _roomRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Room>> GetAllAsync(int page, int pageSize)
        {
            int skip = (page - 1) * pageSize;
            int take = pageSize;

            return await _roomRepository.GetAllAsync(skip, take);
        }

        public async Task CreateAsync(Room room)
        {
            await _roomRepository.AddAsync(room);
            await _roomRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, Room updatedRoom)
        {
            var existing = await _roomRepository.GetByIdAsync(id);

            if (existing == null)
                throw new Exception("Room not found");

            existing.RoomNumber = updatedRoom.RoomNumber;
            existing.Type = updatedRoom.Type;
            existing.PricePerNight = updatedRoom.PricePerNight;
            existing.IsAvailable = updatedRoom.IsAvailable;

            _roomRepository.Update(existing);
            await _roomRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _roomRepository.GetByIdAsync(id);

            if (existing == null)
                throw new Exception("Room not found");

            _roomRepository.Delete(existing);
            await _roomRepository.SaveChangesAsync();
        }
    }
}