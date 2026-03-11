using ZaminHotel.Application.DTOs;
using ZaminHotel.Application.Interfaces;
using ZaminHotel.Domain.Entities;

namespace ZaminHotel.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;

        public BookingService(
            IBookingRepository bookingRepository,
            IRoomRepository roomRepository)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
        }

        public async Task<Booking> CreateBookingAsync(
            int userId,
            int roomId,
            DateTime checkIn,
            DateTime checkOut)
        {
            checkIn = DateTime.SpecifyKind(checkIn.Date, DateTimeKind.Utc);
            checkOut = DateTime.SpecifyKind(checkOut.Date, DateTimeKind.Utc);

            if (checkOut <= checkIn)
                throw new ArgumentException("Check-out must be after check-in.");

            if (checkIn.Date < DateTime.UtcNow.Date)
                throw new ArgumentException("Check-in cannot be in the past.");

            var nights = (checkOut - checkIn).Days;

            if (nights < 1)
                throw new ArgumentException("Minimum stay is 1 night.");

            var room = await _roomRepository.GetByIdAsync(roomId);

            if (room == null || room.IsDeleted)
                throw new KeyNotFoundException("Room not found.");

            var overlappingBookings =
                await _bookingRepository.GetOverlappingBookingAsync(
                    roomId,
                    checkIn,
                    checkOut);

            if (overlappingBookings.Any())
                throw new InvalidOperationException("Room is not available for selected dates.");

            var totalPrice = nights * room.PricePerNight;

            var booking = new Booking
            {
                UserId = userId,
                RoomId = roomId,
                CheckInDate = checkIn,
                CheckOutDate = checkOut,
                TotalPrice = totalPrice,
                Status = "Pending"
            };

            await _bookingRepository.AddAsync(booking);
            await _bookingRepository.SaveChangesAsync();

            return booking;
        }

        public async Task<IEnumerable<BookingResponseDto>> GetMyBookingsAsync(int userId)
        {
            var bookings = await _bookingRepository.GetByUserIdAsync(userId);

            return bookings.Select(b => new BookingResponseDto
            {
                Id = b.Id,
                RoomNumber = b.Room.RoomNumber,
                CheckInDate = b.CheckInDate,
                CheckOutDate = b.CheckOutDate,
                TotalPrice = b.TotalPrice,
                Status = b.Status
            });
        }

        public async Task<IEnumerable<BookingResponseDto>> GetAllBookingsAsync(int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;

            var bookings = await _bookingRepository.GetAllAsync(skip, pageSize);

            return bookings.Select(b => new BookingResponseDto
            {
                Id = b.Id,
                RoomNumber = b.Room.RoomNumber,
                CheckInDate = b.CheckInDate,
                CheckOutDate = b.CheckOutDate,
                TotalPrice = b.TotalPrice,
                Status = b.Status
            });
        }

        public async Task CancelBookingAsync(int bookingId, int userId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if (booking == null || booking.IsDeleted)
                throw new KeyNotFoundException("Booking not found.");
            if (booking.UserId != userId)
                throw new UnauthorizedAccessException("You can only cancel your own bookings.");
            if (booking.Status == "Cancelled")
                throw new InvalidOperationException("Booking is already cancelled.");
            booking.Status = "Cancelled";
            _bookingRepository.Update(booking);
            await _bookingRepository.SaveChangesAsync();
        }
    }
}