using Demo.Restuarants.API.Models;
using Demo.Restuarants.Shared.Models;

namespace Demo.Restuarants.API.Extensions;

internal static class RestuarantMappers
{
    internal static FilterQueryParametersBO ToFilterQueryParametersBO(this FilterQueryParameters parameters)
    {
        return new FilterQueryParametersBO(
            parameters.Names,
            parameters.CuisineType
        )
        {
            Page = parameters.Page ?? 1,
            PageSize = parameters.PageSize ?? 25
        };
    }

    internal static CreateRestuarantRequestBO ToCreateRestuarantRequestBO(this CreateRestuarantRequest request)
    {
        return new CreateRestuarantRequestBO(
            request.Name,
            request.CuisineType,
            request.Website,
            request.Phone,
            request.Address.ToCreateLocationRequestBO(),
            request.BusinessHours is null ? null : [.. request.BusinessHours.Select(_ => _.ToCreateBusinessHourRequestBO())]
        );
    }

    internal static CreateLocationRequestBO ToCreateLocationRequestBO(this CreateLocationRequest request)
    {
        return new CreateLocationRequestBO(
            request.Street,
            request.City,
            request.State,
            request.Country,
            request.ZipCode
        );
    }

    internal static CreateBusinessHourRequestBO ToCreateBusinessHourRequestBO(this CreateBusinessHourRequest request)
    {
        return new CreateBusinessHourRequestBO(
            request.DayOfWeek,
            request.OpenTime,
            request.CloseTime
        );
    }

    internal static UpdateRestuarantRequestBO ToUpdateRestuarantRequestBO(this UpdateRestuarantRequest request)
    {
        return new UpdateRestuarantRequestBO(
            request.Name,
            request.CuisineType,
            request.Website,
            request.Phone,
            request.Address?.ToUpdateLocationRequestBO()
        );
    }

    internal static UpdateLocationRequestBO ToUpdateLocationRequestBO(this UpdateLocationRequest request)
    {
        return new UpdateLocationRequestBO(
            request.Street,
            request.City,
            request.State,
            request.Country,
            request.ZipCode
        );
    }

    internal static UpdateBusinessHourRequestBO ToUpdateBusinessHourRequestBO(this UpdateBusinessHourRequest request)
    {
        return new UpdateBusinessHourRequestBO(
            request.DayOfWeek,
            request.OpenTime,
            request.CloseTime
        );
    }
}
