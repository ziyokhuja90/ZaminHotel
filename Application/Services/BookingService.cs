using ZaminHotel.Application.Interfaces;
using ZaminHotel.Domain.Entities;

namespace ZaminHotel.Application.Services
{
    public class BookingService : IBookinService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;

        public BookingService(IBookingRepository bookingRepository, IRoomRepository roomRepository)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
        }

        public async Task<Booking> CreateBookingAsync(
            int userId,
            int roomId,
            DateTime CheckIn,
            DateTime CheckOut)
        {
            CheckIn = DateTime.SpecifyKind(CheckIn, DateTimeKind.Utc);
            CheckOut = DateTime.SpecifyKind(CheckOut, DateTimeKind.Utc);
            // Validate dates
            if (CheckOut <= CheckIn)
                throw new ArgumentException("Check-out date must be after check-in date");

            if (CheckIn.Date < DateTime.UtcNow.Date)
                throw new ArgumentException("Check-in date cannot be in the past");

            var nights = (CheckOut - CheckIn).Days;

            if (nights < 1)
                throw new InvalidOperationException("Booking must be for at least one night");

            // Check room availability
            var room = await _roomRepository.GetByIdAsync(roomId);

            if (room == null || room.IsDeleted)
                throw new KeyNotFoundException("Room not found");

            // check overlapping bookings
            var overlappingBooking = await _bookingRepository.GetOverlappingBookingAsync(roomId, CheckIn, CheckOut);

            if (overlappingBooking.Any())
                throw new InvalidOperationException("Room is not available...");
            // Calculate total price
            var totalPrice = room.PricePerNight * nights;

            // Create booking
            var booking  = new Booking
            {
                UserId = userId,
                RoomId = roomId,
                CheckInDate = CheckIn,
                CheckOutDate = CheckOut,
                TotalPrice = totalPrice,
                Status = "Pending"
            };
            await _bookingRepository.AddAsync(booking);
            await _bookingRepository.SaveChangesAsync();
            
            return booking;
        }
    }
}
