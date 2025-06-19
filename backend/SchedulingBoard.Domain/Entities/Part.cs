using SchedulingBoard.Domain.Common;

namespace SchedulingBoard.Domain.Entities;

public class Part : BaseEntity
{
    public string PartNumber { get; set; } = string.Empty;
    public string PartDescription { get; set; } = string.Empty;
    public string JobType { get; set; } = string.Empty;
    public string JobDescription { get; set; } = string.Empty;
    public bool IsQualification { get; set; }
    public bool IsActive { get; set; } = true;
    public decimal StandardCost { get; set; }
    public string UnitOfMeasure { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<ProjectPart> ProjectParts { get; set; } = new List<ProjectPart>();
    public ICollection<PartReference> ParentParts { get; set; } = new List<PartReference>();
    public ICollection<PartReference> ChildParts { get; set; } = new List<PartReference>();
}