using Demo.Restuarants.Infrastructure.MongoDb.Documents;
using Demo.Restuarants.Shared.Models;

namespace Demo.Restuarants.Infrastructure.MongoDb.Extensions;

internal static class RestuarantDocumentExtensions
{
    internal static RestuarantBO ToRestuarant(this RestuarantDocument doc)
    {
        return new RestuarantBO(
            doc.Id,
            doc.Name,
            doc.CuisineType,
            doc.Website is null ? null : new Uri(doc.Website),
            doc.Phone,
            doc.Address.ToLocation(),
            [.. doc.BusinessHours.Select(_ => _.ToRestuarantBusinessHourBO())]
        );
    }

    internal static LocationBO ToLocation(this LocationDocument doc)
    {
        return new LocationBO(
            doc.Street,
            doc.City,
            doc.State,
            doc.Country,
            doc.ZipCode
        );
    }

    internal static RestuarantBusinessHourBO ToRestuarantBusinessHourBO(this BusinessHourDocument doc)
    {
        return new RestuarantBusinessHourBO(
            doc.Id,
            Enum.Parse<DayOfWeek>(doc.DayOfWeek),
            doc.OpenTime,
            doc.CloseTime
        );
    }

    internal static RestuarantDocument ToRestuarantDocument(this RestuarantBO model)
    {
        return new RestuarantDocument()
        {
            Id = model.Id,
            Name = model.Name,
            CuisineType = model.CuisineType,
            Website = model.Website?.ToString(),
            Phone = model.Phone,
            Address = model.Address.ToLocationDocument(),
            BusinessHours = [.. model.BusinessHours.Select(_ => _.ToBusinessHourDocument())]
        };
    }

    internal static LocationDocument ToLocationDocument(this LocationBO model)
    {
        return new LocationDocument()
        {
            Street = model.Street,
            City = model.City,
            State = model.State,
            Country = model.Country,
            ZipCode = model.ZipCode
        };
    }

    internal static BusinessHourDocument ToBusinessHourDocument(this RestuarantBusinessHourBO model)
    {
        return new BusinessHourDocument()
        {
            Id = model.Id,
            DayOfWeek = model.DayOfWeek.ToString(),
            OpenTime = model.OpenTime,
            CloseTime = model.CloseTime
        };
    }
}
