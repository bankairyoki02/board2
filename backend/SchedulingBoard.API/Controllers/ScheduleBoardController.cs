using Microsoft.AspNetCore.Mvc;
using SchedulingBoard.Application.DTOs;
using SchedulingBoard.Application.Queries;
using MediatR;

namespace SchedulingBoard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScheduleBoardController : ControllerBase
{
    private readonly IMediator _mediator;

    public ScheduleBoardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ScheduleBoardDto>> GetScheduleBoard(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] string? warehouses = null,
        [FromQuery] bool includeProposals = true,
        [FromQuery] bool showDetail = true,
        [FromQuery] bool showFuture = true)
    {
        var query = new GetScheduleBoardQuery
        {
            StartDate = startDate ?? DateTime.Today.AddDays(-30),
            EndDate = endDate ?? DateTime.Today.AddDays(90),
            IncludedWarehouses = string.IsNullOrEmpty(warehouses) 
                ? new List<string>() 
                : warehouses.Split(',').ToList(),
            IncludeProposals = includeProposals,
            ShowDetail = showDetail,
            ShowFuture = showFuture
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }
}

[ApiController]
[Route("api/[controller]")]
public class WarehousesController : ControllerBase
{
    private readonly IMediator _mediator;

    public WarehousesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<WarehouseDto>>> GetWarehouses(
        [FromQuery] bool includeInactive = false)
    {
        var query = new GetWarehousesQuery { IncludeInactive = includeInactive };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("groups")]
    public async Task<ActionResult<List<WarehouseGroupDto>>> GetWarehouseGroups(
        [FromQuery] bool includeInactive = false)
    {
        var query = new GetWarehouseGroupsQuery { IncludeInactive = includeInactive };
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}