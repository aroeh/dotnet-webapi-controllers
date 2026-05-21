using Demo.Restuarants.API.Models;
using Demo.Restuarants.Shared.Models;

namespace Demo.Restuarants.Client.MVC.Models;

public class RestuarantListViewModel
{
    public PaginationResponse<RestuarantBO>? Results { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 25;

    public string? NameFilter { get; set; }

    public string? CuisineFilter { get; set; }

    public FilterQueryParameters ToQueryParameters() => new()
    {
        Page = Page,
        PageSize = PageSize,
        Names = string.IsNullOrWhiteSpace(NameFilter)
            ? null
            : [.. NameFilter.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)],
        CuisineType = string.IsNullOrWhiteSpace(CuisineFilter) ? null : CuisineFilter
    };
}
