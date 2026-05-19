namespace Demo.Restuarants.Shared.Models;

public record UpdateBusinessHourRequestBO
(
    DayOfWeek? DayOfWeek,
    TimeOnly? OpenTime,
    TimeOnly? CloseTime
);
