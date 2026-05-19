namespace Demo.Restuarants.API.Models;

public record CreateBusinessHourRequest
{
    public DayOfWeek DayOfWeek { get; set; }
    
    public TimeOnly OpenTime { get; set; }
    
    public TimeOnly CloseTime { get; set; }
}
