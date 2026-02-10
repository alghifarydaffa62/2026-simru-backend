namespace SimruBackend.DTO
{
    public class RoomResponseDTO
    {
        public int Id { get; set; }
        public string RoomCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; } 
        public CurrentReservationDTO? CurrentReservation { get; set; }
    }

    public class CurrentReservationDTO
    {
        public string BorrowerName { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
    }
}