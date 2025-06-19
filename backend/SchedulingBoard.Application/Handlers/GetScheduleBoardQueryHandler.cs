using MediatR;
using Microsoft.EntityFrameworkCore;
using SchedulingBoard.Application.DTOs;
using SchedulingBoard.Application.Interfaces;
using SchedulingBoard.Application.Queries;

namespace SchedulingBoard.Application.Handlers;

public class GetScheduleBoardQueryHandler : IRequestHandler<GetScheduleBoardQuery, ScheduleBoardDto>
{
    private readonly ISchedulingBoardDbContext _context;

    public GetScheduleBoardQueryHandler(ISchedulingBoardDbContext context)
    {
        _context = context;
    }

    public async Task<ScheduleBoardDto> Handle(GetScheduleBoardQuery request, CancellationToken cancellationToken)
    {
        // Get schedule items with related data
        var scheduleItemsQuery = _context.ScheduleItems
            .Include(si => si.Project)
            .Include(si => si.Warehouse)
            .Include(si => si.Part)
            .Where(si => si.ScheduledDate >= request.StartDate && si.ScheduledDate <= request.EndDate);

        // Apply warehouse filter if specified
        if (request.IncludedWarehouses.Any())
        {
            scheduleItemsQuery = scheduleItemsQuery
                .Where(si => request.IncludedWarehouses.Contains(si.Warehouse.WarehouseCode));
        }

        // Apply proposal filter
        if (!request.IncludeProposals)
        {
            scheduleItemsQuery = scheduleItemsQuery.Where(si => !si.IsProposal);
        }

        // Apply future filter
        if (!request.ShowFuture)
        {
            scheduleItemsQuery = scheduleItemsQuery.Where(si => si.ScheduledDate <= DateTime.Today);
        }

        var scheduleItems = await scheduleItemsQuery.ToListAsync(cancellationToken);

        // Get warehouses
        var warehouses = await _context.Warehouses
            .Where(w => w.IsActive)
            .ToListAsync(cancellationToken);

        // Get projects
        var projects = await _context.Projects
            .Where(p => p.IsActive)
            .ToListAsync(cancellationToken);

        // Map to DTOs
        var scheduleItemDtos = scheduleItems.Select(si => new ScheduleItemDto
        {
            Id = si.Id,
            ProjectId = si.ProjectId,
            ProjectNumber = si.Project.ProjectNumber,
            ProjectName = si.Project.ProjectName,
            WarehouseId = si.WarehouseId,
            WarehouseCode = si.Warehouse.WarehouseCode,
            WarehouseName = si.Warehouse.CompanyDesc,
            PartId = si.PartId,
            PartNumber = si.Part?.PartNumber,
            PartDescription = si.Part?.PartDescription,
            ScheduledDate = si.ScheduledDate,
            Quantity = si.Quantity,
            Status = si.Status,
            IsLate = si.IsLate,
            IsProposal = si.IsProposal,
            Notes = si.Notes
        }).ToList();

        var warehouseDtos = warehouses.Select(w => new WarehouseDto
        {
            Id = w.Id,
            WarehouseCode = w.WarehouseCode,
            CompanyCode = w.CompanyCode,
            CompanyDesc = w.CompanyDesc,
            CountryCode = w.CountryCode,
            LocationCode = w.LocationCode,
            Abbreviation = w.Abbreviation,
            Division = w.Division,
            DivisionDesc = w.DivisionDesc,
            DefaultCrewOpsEmpno = w.DefaultCrewOpsEmpno,
            DefaultCrewOpsName = w.DefaultCrewOpsName,
            DefaultCrewOpsEmail = w.DefaultCrewOpsEmail,
            TouringRevenueGroup = w.TouringRevenueGroup,
            IsActive = w.IsActive
        }).ToList();

        var projectDtos = projects.Select(p => new ProjectDto
        {
            Id = p.Id,
            ProjectNumber = p.ProjectNumber,
            ProjectName = p.ProjectName,
            Description = p.Description,
            StartDate = p.StartDate,
            EndDate = p.EndDate,
            Status = p.Status,
            EntityNo = p.EntityNo,
            EntityDesc = p.EntityDesc,
            IsActive = p.IsActive
        }).ToList();

        return new ScheduleBoardDto
        {
            ScheduleItems = scheduleItemDtos,
            Warehouses = warehouseDtos,
            Projects = projectDtos,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };
    }
}