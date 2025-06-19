namespace SchedulingBoard.Application.DTOs;

public class ScheduleItemDto
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string ProjectNumber { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public int WarehouseId { get; set; }
    public string WarehouseCode { get; set; } = string.Empty;
    public string WarehouseName { get; set; } = string.Empty;
    public int? PartId { get; set; }
    public string? PartNumber { get; set; }
    public string? PartDescription { get; set; }
    public DateTime ScheduledDate { get; set; }
    public int Quantity { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsLate { get; set; }
    public bool IsProposal { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public class ScheduleBoardDto
{
    public List<ScheduleItemDto> ScheduleItems { get; set; } = new();
    public List<WarehouseDto> Warehouses { get; set; } = new();
    public List<ProjectDto> Projects { get; set; } = new();
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}