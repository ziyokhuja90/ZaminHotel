using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

using ZaminHotel.Application.Interfaces;

namespace ZaminHotel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Customer")]
    public class  BookingController : ControllerBase
    {
        private readonly IBookinService _bookingService;

        public BookingController(IBookinService bookingService)
        {
            _bookingService = bookingService;
        }
        [HttpPost]
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
    }
}
