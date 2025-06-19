using SchedulingBoard.Domain.Common;

namespace SchedulingBoard.Domain.Entities;

public class ScheduleItem : BaseEntity
{
    public int ProjectId { get; set; }
    public int WarehouseId { get; set; }
    public int? PartId { get; set; }
    public DateTime ScheduledDate { get; set; }
    public int Quantity { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsLate { get; set; }
    public bool IsProposal { get; set; }
    public string Notes { get; set; } = string.Empty;

    // Navigation properties
    public Project Project { get; set; } = null!;
    public Warehouse Warehouse { get; set; } = null!;
    public Part? Part { get; set; }
}