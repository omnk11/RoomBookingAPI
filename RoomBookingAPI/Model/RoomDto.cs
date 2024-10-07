namespace RoomBookingAPI.Model
{
    // RoomBookingAPI/Models/RoomDto.cs
    using System.ComponentModel.DataAnnotations;

    public class RoomDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public decimal Price { get; set; }

        public IFormFile Image { get; set; }
    }

}
