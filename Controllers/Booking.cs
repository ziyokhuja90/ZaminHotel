using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

using ZaminHotel.Application.Interfaces;

namespace ZaminHotel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Create(
            int roomId,
            DateTime CheckIn,
            DateTime CheckOut)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim.Value);

            var booking = await _bookingService.CreateBookingAsync(userId, roomId, CheckIn, CheckOut);

            return Ok(booking);
        }

        [HttpGet("my")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyBookings()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();
            var userId = int.Parse(userIdClaim.Value);
            var bookings = await _bookingService.GetMyBookingsAsync(userId);
            return Ok(bookings);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 10)
        {
            var bookings = await _bookingService.GetAllBookingsAsync(page, pageSize);
            return Ok(bookings);
        }

        [HttpPut("{id}/cancel")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Cancel(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();
            var userId = int.Parse(userIdClaim.Value);
            await _bookingService.CancelBookingAsync(id, userId);
            return Ok(new {message = "Booking canceled successfully."});
        }
    }
}
