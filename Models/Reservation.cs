using System.ComponentModel.DataAnnotations; 

namespace SimruBackend.Models {
    public class Reservation {
        public int Id { get; set; }

        [Required]
        public int RoomId { get; set; }
        public Room? Room { get; set; }

        [Required]
        public string BorrowerName { get; set; } = string.Empty;

        [Required]
        public DateTime BorrowDate { get; set; }
        public string Status { get; set; } = "Active";
        public bool IsDeleted { get; set; } = false;
    }
}