using System;

namespace qrschool_deckstop.Models
{
    public class InventoryLog
    {
        public int Id { get; set; }
        public string ObjectType { get; set; } = string.Empty;
        public int ObjectId { get; set; }
        public string Operation { get; set; } = string.Empty;
        public int? RoomId { get; set; }
        public int? UserId { get; set; }
        public string Comment { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public string SyncStatus { get; set; } = "synced";
        public DateTime CreatedAt { get; set; }
    }
}
