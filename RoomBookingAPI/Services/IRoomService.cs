using Microsoft.EntityFrameworkCore;
using RoomBookingAPI.Data;
using RoomBookingAPI.Model;

namespace RoomBookingAPI.Services
{
    // RoomBookingAPI/Services/IRoomService.cs
    public interface IRoomService
    {
        Task<IEnumerable<Room>> GetAllRoomsAsync();
        Task<IEnumerable<Room>> SearchRoomsAsync(string city);
        Task AddRoomAsync(RoomDto roomDto, int ownerId);
        // Additional methods for updating and deleting rooms
    }

// RoomBookingAPI/Services/RoomService.cs

public class RoomService : IRoomService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public RoomService(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IEnumerable<Room>> GetAllRoomsAsync()
        {
            return await _context.Rooms.Include(r => r.Owner).ToListAsync();
        }

        public async Task<IEnumerable<Room>> SearchRoomsAsync(string city)
        {
            return await _context.Rooms
                .Where(r => r.Location.Contains(city))
                .Include(r => r.Owner)
                .ToListAsync();
        }

        public async Task AddRoomAsync(RoomDto roomDto, int ownerId)
        {
            string imagePath = null;
            if (roomDto.Image != null)
            {
                var uploads = Path.Combine(_environment.WebRootPath, "images");
                if (!Directory.Exists(uploads))
                    Directory.CreateDirectory(uploads);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(roomDto.Image.FileName);
                var filePath = Path.Combine(uploads, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await roomDto.Image.CopyToAsync(fileStream);
                }
                imagePath = $"/images/{fileName}";
            }

            var room = new Room
            {
                Title = roomDto.Title,
                Location = roomDto.Location,
                Price = roomDto.Price,
                ImageUrl = imagePath,
                OwnerId = ownerId
            };

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
        }
    }

}
