namespace Demo.Restuarants.API.Models;

public record UpdateBusinessHourRequest
{
    public DayOfWeek? DayOfWeek { get; set; }

    public TimeOnly? OpenTime { get; set; }

    public TimeOnly? CloseTime { get; set; }
}
