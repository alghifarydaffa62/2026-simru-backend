using System.ComponentModel.DataAnnotations; 

namespace SimruBackend.Models {
    public enum ReservationStatus {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }

    public class Reservation {
        public int Id { get; set; }

        [Required]
        public int RoomId { get; set; }
        public Room? Room { get; set; }

        [Required]
        public string BorrowerName { get; set; } = string.Empty;

        [Required]
        public DateTime BorrowDate { get; set; }
        public string Purpose { get; set; } = string.Empty;
        public ReservationStatus Status { get; set; } 
        public bool IsDeleted { get; set; } = false;
    }
}