using System.ComponentModel.DataAnnotations;

namespace Demo.Restuarants.Client.MVC.Models;

public class BusinessHourFormViewModel
{
    public string RestuarantId { get; set; } = string.Empty;

    public string? BusinessHourId { get; set; }

    [Required]
    [Display(Name = "Day of Week")]
    public DayOfWeek DayOfWeek { get; set; }

    [Required]
    [Display(Name = "Open Time")]
    [DataType(DataType.Time)]
    public TimeOnly OpenTime { get; set; }

    [Required]
    [Display(Name = "Close Time")]
    [DataType(DataType.Time)]
    public TimeOnly CloseTime { get; set; }
}
