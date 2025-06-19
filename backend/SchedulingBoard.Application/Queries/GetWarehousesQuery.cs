using MediatR;
using SchedulingBoard.Application.DTOs;

namespace SchedulingBoard.Application.Queries;

public class GetWarehousesQuery : IRequest<List<WarehouseDto>>
{
    public bool IncludeInactive { get; set; } = false;
}

public class GetWarehouseGroupsQuery : IRequest<List<WarehouseGroupDto>>
{
    public bool IncludeInactive { get; set; } = false;
}