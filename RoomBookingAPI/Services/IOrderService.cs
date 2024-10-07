using Microsoft.EntityFrameworkCore;
using RoomBookingAPI.Data;
using RoomBookingAPI.Model;

namespace RoomBookingAPI.Services
{// RoomBookingAPI/Services/IOrderService.cs
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrdersByUserAsync(int userId);
        Task BookRoomAsync(int roomId, int userId);
        Task ProcessOrderAsync(int orderId);
        Task CancelOrderAsync(int orderId);
    }

    // RoomBookingAPI/Services/OrderService.cs

    public class OrderService : IOrderService
    {
        private readonly IEmailService _emailService;

        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Room)
                .ThenInclude(r => r.Owner)
                .ToListAsync();
        }

        public async Task BookRoomAsync(int roomId, int userId)
        {
            var order = new Order
            {
                RoomId = roomId,
                UserId = userId,
                BookingDate = DateTime.UtcNow,
                Status = "Pending"
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var room = await _context.Rooms.Include(r => r.Owner).FirstOrDefaultAsync(r => r.Id == roomId);
            var user = await _context.Users.FindAsync(userId);

            var subject = "Room Booking Confirmation";
            var body = $"<p>Hello {user.Email},</p>" +
                       $"<p>You have successfully booked the room: {room.Title} located at {room.Location}.</p>" +
                       $"<p>Price: {room.Price}</p>" +
                       $"<p>Landlord Details: {room.Owner.Email}</p>" +
                       $"<p>Please pay the amount during check-in.</p>";

            await _emailService.SendEmailAsync(user.Email, subject, body);
        }

        public async Task ProcessOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = "Processed";
                await _context.SaveChangesAsync();
            }
        }

        public async Task CancelOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = "Cancelled";
                await _context.SaveChangesAsync();
            }
        }
    }

}
