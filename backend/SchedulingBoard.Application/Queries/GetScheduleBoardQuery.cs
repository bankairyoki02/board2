using MediatR;
using SchedulingBoard.Application.DTOs;

namespace SchedulingBoard.Application.Queries;

public class GetScheduleBoardQuery : IRequest<ScheduleBoardDto>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<string> IncludedWarehouses { get; set; } = new();
    public bool IncludeProposals { get; set; } = true;
    public bool ShowDetail { get; set; } = true;
    public bool ShowFuture { get; set; } = true;
}