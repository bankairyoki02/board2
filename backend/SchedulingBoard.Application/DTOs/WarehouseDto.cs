namespace SchedulingBoard.Application.DTOs;

public class WarehouseDto
{
    public int Id { get; set; }
    public string WarehouseCode { get; set; } = string.Empty;
    public string CompanyCode { get; set; } = string.Empty;
    public string CompanyDesc { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public string LocationCode { get; set; } = string.Empty;
    public string Abbreviation { get; set; } = string.Empty;
    public string Division { get; set; } = string.Empty;
    public string DivisionDesc { get; set; } = string.Empty;
    public string DefaultCrewOpsEmpno { get; set; } = string.Empty;
    public string DefaultCrewOpsName { get; set; } = string.Empty;
    public string DefaultCrewOpsEmail { get; set; } = string.Empty;
    public string TouringRevenueGroup { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class WarehouseGroupDto
{
    public int Id { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public List<WarehouseDto> Warehouses { get; set; } = new();
}