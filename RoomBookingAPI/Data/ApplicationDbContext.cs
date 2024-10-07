using Microsoft.EntityFrameworkCore;
using RoomBookingAPI.Model;
// RoomBookingAPI/Data/ApplicationDbContext.cs
namespace RoomBookingAPI.Data
{


    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Order> Orders { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            // Configure Orders to Rooms relationship without cascading deletes
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Room)
                .WithMany()
                .HasForeignKey(o => o.RoomId)
                .OnDelete(DeleteBehavior.NoAction);  // Disable cascading deletes

            // Configure Orders to Users relationship without cascading deletes
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.NoAction);  // Disable cascading deletes
        }

    }


}










