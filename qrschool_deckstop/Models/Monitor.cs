using System;

namespace qrschool_deckstop.Models
{
    public class Monitor
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string InventoryNo { get; set; } = string.Empty;
        public int? RoomId { get; set; }
        public int? ComputerId { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public decimal? DiagonalInch { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public decimal? Cost { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? WarrantyUntil { get; set; }
        public int? ResponsibleEmployeeId { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string SyncStatus { get; set; } = "synced";

        public string RoomName { get; set; } = string.Empty;
        public string ResponsibleEmployee { get; set; } = string.Empty;
    }
}
