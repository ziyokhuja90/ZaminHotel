using ZaminHotel.Domain.Common;

namespace ZaminHotel.Domain.Entities
{
    public class Room: BaseEntity
    {
        public string RoomNumber { get; set; } = null!;
        public string Type { get; set; } = null!;
        public decimal PricePerNight { get; set; }
        public bool IsAvailable { get; set; } = true;


    }
}
