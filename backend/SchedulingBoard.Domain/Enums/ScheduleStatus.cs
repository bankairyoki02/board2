namespace SchedulingBoard.Domain.Enums;

public enum ScheduleStatus
{
    Planned,
    InProgress,
    Completed,
    Cancelled,
    OnHold,
    Late
}

public enum ProjectStatus
{
    Active,
    Completed,
    Cancelled,
    OnHold,
    Planning
}

public enum PartReferenceType
{
    Substitute = 5,
    Alternative = 1,
    Component = 2,
    Assembly = 3
}