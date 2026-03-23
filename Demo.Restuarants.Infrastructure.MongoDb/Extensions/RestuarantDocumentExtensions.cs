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
            doc.Address.ToLocation()
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
}
