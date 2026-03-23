namespace Demo.Restuarants.Shared.Models;

public record RestuarantBO
(
    string Id,
    string Name,
    string CuisineType,
    Uri? Website,
    string Phone,
    LocationBO Address
);
