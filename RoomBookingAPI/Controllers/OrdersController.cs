using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using RoomBookingAPI.Services;
using RoomBookingAPI.Model;

namespace RoomBookingAPI.Controllers
{
    // RoomBookingAPI/Controllers/OrdersController.cs
   

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("myorders")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var orders = await _orderService.GetOrdersByUserAsync(userId);
            return Ok(orders);
        }

        [HttpPost("book")]
        public async Task<IActionResult> BookRoom([FromBody] BookRoomDto bookDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await _orderService.BookRoomAsync(bookDto.RoomId, userId);
            // TODO: Send email to landlord
            return Ok("Room booked successfully.");
        }

        [HttpPost("{orderId}/process")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> ProcessOrder(int orderId)
        {
            await _orderService.ProcessOrderAsync(orderId);
            return Ok("Order processed.");
        }

        [HttpPost("{orderId}/cancel")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            await _orderService.CancelOrderAsync(orderId);
            return Ok("Order cancelled.");
        }
    }

}
