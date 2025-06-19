using MediatR;
using Microsoft.EntityFrameworkCore;
using SchedulingBoard.Application.DTOs;
using SchedulingBoard.Application.Interfaces;
using SchedulingBoard.Application.Queries;

namespace SchedulingBoard.Application.Handlers;

public class GetWarehousesQueryHandler : IRequestHandler<GetWarehousesQuery, List<WarehouseDto>>
{
    private readonly ISchedulingBoardDbContext _context;

    public GetWarehousesQueryHandler(ISchedulingBoardDbContext context)
    {
        _context = context;
    }

    public async Task<List<WarehouseDto>> Handle(GetWarehousesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Warehouses.AsQueryable();

        if (!request.IncludeInactive)
        {
            query = query.Where(w => w.IsActive);
        }

        var warehouses = await query.ToListAsync(cancellationToken);

        return warehouses.Select(w => new WarehouseDto
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
    }
}

public class GetWarehouseGroupsQueryHandler : IRequestHandler<GetWarehouseGroupsQuery, List<WarehouseGroupDto>>
{
    private readonly ISchedulingBoardDbContext _context;

    public GetWarehouseGroupsQueryHandler(ISchedulingBoardDbContext context)
    {
        _context = context;
    }

    public async Task<List<WarehouseGroupDto>> Handle(GetWarehouseGroupsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.WarehouseGroups
            .Include(wg => wg.Warehouses)
            .AsQueryable();

        if (!request.IncludeInactive)
        {
            query = query.Where(wg => wg.IsActive);
        }

        var warehouseGroups = await query.ToListAsync(cancellationToken);

        return warehouseGroups.Select(wg => new WarehouseGroupDto
        {
            Id = wg.Id,
            GroupName = wg.GroupName,
            Description = wg.Description,
            IsActive = wg.IsActive,
            Warehouses = wg.Warehouses.Select(w => new WarehouseDto
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
            }).ToList()
        }).ToList();
    }
}