using SchedulingBoard.Domain.Common;

namespace SchedulingBoard.Domain.Entities;

public class WarehouseGroup : BaseEntity
{
    public string GroupName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
}