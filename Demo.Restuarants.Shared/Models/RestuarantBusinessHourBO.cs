namespace Demo.Restuarants.Shared.Models;

public record RestuarantBusinessHourBO
(
    string Id,
    DayOfWeek DayOfWeek,
    TimeOnly OpenTime,
    TimeOnly CloseTime
);
