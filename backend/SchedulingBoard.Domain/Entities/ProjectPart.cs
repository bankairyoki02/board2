using SchedulingBoard.Domain.Common;

namespace SchedulingBoard.Domain.Entities;

public class ProjectPart : BaseEntity
{
    public int ProjectId { get; set; }
    public int PartId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public decimal TotalCost { get; set; }
    public DateTime RequiredDate { get; set; }
    public string Status { get; set; } = string.Empty;

    // Navigation properties
    public Project Project { get; set; } = null!;
    public Part Part { get; set; } = null!;
}