
namespace SimruBackend.DTO
{
    public class ReservationRequestDTO 
    {
        public int RoomId { get; set; }
        public string BorrowerName { get; set; } = string.Empty;
        public DateTime BorrowDate { get; set; }
        public string Purpose { get; set; } = string.Empty;
        public int Status { get; set; }
    }
}