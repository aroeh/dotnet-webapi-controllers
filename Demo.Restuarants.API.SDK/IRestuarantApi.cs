using Demo.Restuarants.API.Models;
using Demo.Restuarants.Shared.Models;
using Refit;

namespace Demo.Restuarants.API.SDK;

public interface IRestuarantApi
{
    [Get("/api/restuarant" + $"{ApiVersions.QueryParamVersion}")]
    Task<PaginationResponse<RestuarantBO>> QueryRestuarants(FilterQueryParameters queryParameters);

    [Get("/api/restuarant/{restuarantId}" + $"{ApiVersions.QueryParamVersion}")]
    Task<PaginationResponse<RestuarantBO>> GetRestuarant(string restuarantId);
}
