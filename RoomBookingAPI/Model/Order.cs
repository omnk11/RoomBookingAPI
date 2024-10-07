// RoomBookingAPI/Models/Order.cs
using System.ComponentModel.DataAnnotations;

namespace RoomBookingAPI.Model
{
   

    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RoomId { get; set; }
        public Room Room { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public string Status { get; set; } // "Pending", "Processed", "Cancelled"
    }

}
