using Demo.Restuarants.Shared.Models;

namespace Demo.Restuarants.Core.Extensions;

internal static class RestuarantExtensions
{
    internal static RestuarantBO MapToRestuarant(this CreateRestuarantRequestBO request, string id)
    {
        return new RestuarantBO(
            id,
            request.Name,
            request.CuisineType,
            request.Website,
            request.Phone,
            request.Address.MapToLocation()
        );
    }

    internal static LocationBO MapToLocation(this CreateLocationRequestBO request)
    {
        return new LocationBO(
            request.Street,
            request.City,
            request.State,
            request.Country,
            request.ZipCode
        );
    }
}
