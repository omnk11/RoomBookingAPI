using Microsoft.EntityFrameworkCore;
using RoomBookingAPI.Data;
using RoomBookingAPI.Model;

namespace RoomBookingAPI.Services
{
    // RoomBookingAPI/Services/IUserService.cs
    public interface IUserService
    {
        Task<User> GetUserByEmailAsync(string email);
        Task CreateUserAsync(User user);
    }

    // RoomBookingAPI/Services/UserService.cs

    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }

}
