using SchedulingBoard.Domain.Common;

namespace SchedulingBoard.Domain.Entities;

public class PartReference : BaseEntity
{
    public int ParentPartId { get; set; }
    public int ChildPartId { get; set; }
    public decimal Factor { get; set; } = 1.0m;
    public int ReferenceTypeId { get; set; }
    public string ReferenceType { get; set; } = string.Empty; // e.g., "Substitute", "Alternative"

    // Navigation properties
    public Part ParentPart { get; set; } = null!;
    public Part ChildPart { get; set; } = null!;
}