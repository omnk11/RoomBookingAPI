using System.ComponentModel.DataAnnotations;

namespace RoomBookingAPI.Model
{
    public class BookRoomDto
    {
        [Required]
        public int RoomId { get; set; }
        // Additional fields like booking dates can be added
    }
}
