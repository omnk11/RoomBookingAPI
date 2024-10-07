using System.ComponentModel.DataAnnotations;



namespace RoomBookingAPI.Model

{
    // RoomBookingAPI/Models/User.cs

    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } // "Admin" or "User"

        public ICollection<Order> Orders { get; set; }
        public ICollection<Room> Rooms { get; set; }
    }

}
