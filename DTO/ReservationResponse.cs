
namespace SimruBackend.DTO
{
    public class ReservationResponseDTO 
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public string RoomCode { get; set; } = string.Empty;
        public int RoomCapacity { get; set; }
        public string BorrowerName { get; set; } = string.Empty;
        public DateTime BorrowDate { get; set; } 
        public string Purpose { get; set; } = string.Empty;
        public int Status { get; set; }
    }
}