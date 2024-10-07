using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomBookingAPI.Data;
using RoomBookingAPI.Services;
using RoomBookingAPI.Model;

namespace RoomBookingAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public BookingController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost("BookRoom")]
        public async Task<IActionResult> BookRoom(int roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null)
            {
                return NotFound();
            }

            var booking = new Booking
            {
                RoomId = roomId,
                UserId = User.Identity.Name,
                BookingDate = DateTime.Now,
                Price = room.Price
            };

            //_context.Bookings.Add(booking);
            //await _context.SaveChangesAsync();

            //await _emailService.SendBookingConfirmation(User.Identity.Name, room);

            return Ok(new { message = "Room booked successfully" });
        }
    }

}
