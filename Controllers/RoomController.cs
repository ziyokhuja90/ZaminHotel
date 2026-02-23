using Microsoft.AspNetCore.Mvc;
using ZaminHotel.Application.Interfaces;
using ZaminHotel.Domain.Entities;

namespace ZaminHotel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var room = await _roomService.GetByIdAsync(id);

            if (room == null)
                return NotFound();

            return Ok(room);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 10)
        {
            var rooms = await _roomService.GetAllAsync(page, pageSize);
            return Ok(rooms);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Room room)
        {
            await _roomService.CreateAsync(room);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Room room)
        {
            await _roomService.UpdateAsync(id, room);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _roomService.DeleteAsync(id);
            return Ok();
        }
    }
}