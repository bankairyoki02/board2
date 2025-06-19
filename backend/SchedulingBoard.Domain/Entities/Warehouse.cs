using SchedulingBoard.Domain.Common;

namespace SchedulingBoard.Domain.Entities;

public class Warehouse : BaseEntity
{
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
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<WarehouseGroup> WarehouseGroups { get; set; } = new List<WarehouseGroup>();
    public ICollection<ScheduleItem> ScheduleItems { get; set; } = new List<ScheduleItem>();
}