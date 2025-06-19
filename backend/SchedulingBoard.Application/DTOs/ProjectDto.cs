namespace SchedulingBoard.Application.DTOs;

public class ProjectDto
{
    public int Id { get; set; }
    public string ProjectNumber { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string EntityNo { get; set; } = string.Empty;
    public string EntityDesc { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class ProjectDetailDto : ProjectDto
{
    public List<ScheduleItemDto> ScheduleItems { get; set; } = new();
    public List<ProjectPartDto> ProjectParts { get; set; } = new();
}

public class ProjectPartDto
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public int PartId { get; set; }
    public string PartNumber { get; set; } = string.Empty;
    public string PartDescription { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public decimal TotalCost { get; set; }
    public DateTime RequiredDate { get; set; }
    public string Status { get; set; } = string.Empty;
}