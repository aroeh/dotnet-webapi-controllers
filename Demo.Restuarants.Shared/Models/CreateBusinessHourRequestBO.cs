namespace Demo.Restuarants.Shared.Models;

public record CreateBusinessHourRequestBO
(
    DayOfWeek DayOfWeek,
    TimeOnly OpenTime,
    TimeOnly CloseTime
);
