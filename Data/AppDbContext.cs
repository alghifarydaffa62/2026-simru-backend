using Microsoft.EntityFrameworkCore;
using SimruBackend.Models;

namespace SimruBackend.Data {
    public class AppDbContext : DbContext {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Room>().HasData(
                new Room { Id = 1, RoomCode = "B301", Name = "Gedung D4 B301", Capacity = 50 },
                new Room { Id = 2, RoomCode = "B302", Name = "Gedung D4 B302", Capacity = 50 }
            );

            modelBuilder.Entity<Reservation>().HasData(
                new Reservation {
                    Id = 1,
                    RoomId = 1,
                    BorrowerName = "Silia Julia",
                    BorrowDate = new DateTime(2026, 2, 7, 9, 0, 0),
                    Purpose = "Kebutuhan ruangan himpunan sastra mesin"
                }
            );
        }
    }
}