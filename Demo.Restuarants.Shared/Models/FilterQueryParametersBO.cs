namespace Demo.Restuarants.Shared.Models;

public record FilterQueryParametersBO
(
    string[]? Names,
    string? CuisineType
) : PaginationQueryParametersBO;
