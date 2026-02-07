namespace SimruBackend.Models {
    public class Room {
        public int Id { get; set; }
        public string RoomCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}