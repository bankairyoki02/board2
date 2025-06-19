using SchedulingBoard.Domain.Common;

namespace SchedulingBoard.Domain.Entities;

public class Project : BaseEntity
{
    public string ProjectNumber { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string EntityNo { get; set; } = string.Empty;
    public string EntityDesc { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<ScheduleItem> ScheduleItems { get; set; } = new List<ScheduleItem>();
    public ICollection<ProjectPart> ProjectParts { get; set; } = new List<ProjectPart>();
}