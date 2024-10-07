using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomBookingAPI.Services;
using RoomBookingAPI.Model;

namespace RoomBookingAPI.Controllers

{
    // RoomBookingAPI/Controllers/RoomsController.cs
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRooms()
        {
            var rooms = await _roomService.GetAllRoomsAsync();
            return Ok(rooms);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchRooms(string city)
        {
            var rooms = await _roomService.SearchRoomsAsync(city);
            return Ok(rooms);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> AddRoom([FromForm] RoomDto roomDto)
        {
            var ownerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await _roomService.AddRoomAsync(roomDto, ownerId);
            return Ok("Room added successfully.");
        }

        // Additional endpoints for updating and deleting rooms
    }

}


