namespace RoomBookingAPI.Model
{
    public class Booking
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string UserId { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal Price { get; set; }
    }

}
