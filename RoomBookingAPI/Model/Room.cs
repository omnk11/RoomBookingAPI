using System.ComponentModel.DataAnnotations;


namespace RoomBookingAPI.Model
{
    // RoomBookingAPI/Models/Room.cs

    public class Room
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public int OwnerId { get; set; }
        public User Owner { get; set; }

        public ICollection<Order> Orders { get; set; }
    }


}
