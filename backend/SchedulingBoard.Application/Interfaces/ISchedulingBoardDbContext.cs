using Microsoft.EntityFrameworkCore;
using SchedulingBoard.Domain.Entities;

namespace SchedulingBoard.Application.Interfaces;

public interface ISchedulingBoardDbContext
{
    DbSet<Warehouse> Warehouses { get; }
    DbSet<WarehouseGroup> WarehouseGroups { get; }
    DbSet<Project> Projects { get; }
    DbSet<Part> Parts { get; }
    DbSet<PartReference> PartReferences { get; }
    DbSet<ProjectPart> ProjectParts { get; }
    DbSet<ScheduleItem> ScheduleItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}