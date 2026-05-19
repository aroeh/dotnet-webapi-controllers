using Demo.Restuarants.API.Models;
using Demo.Restuarants.Shared.Models;
using Refit;

namespace Demo.Restuarants.API.SDK;

public interface IRestuarantsApi
{
    [Get("/api/restuarant" + $"{ApiVersions.QueryParamVersion}")]
    Task<PaginationResponse<RestuarantBO>> QueryRestuarantsAsync(FilterQueryParameters queryParameters);

    [Get("/api/restuarant/{restuarantId}" + $"{ApiVersions.QueryParamVersion}")]
    Task<RestuarantBO?> GetRestuarantAsync(string restuarantId);

    [Post("/api/restuarant" + $"{ApiVersions.QueryParamVersion}")]
    Task<RestuarantBO> CreateRestuarantAsync(CreateRestuarantRequest request);

    [Post("/api/restuarant/bulk" + $"{ApiVersions.QueryParamVersion}")]
    Task<RestuarantBO> CreateManyRestuarantsAsync(CreateRestuarantRequest[] request);

    [Put("/api/restuarant/{restuarantId}" + $"{ApiVersions.QueryParamVersion}")]
    Task<RestuarantBO> UpdateRestuarantAsync(string restuarantId, UpdateRestuarantRequest request);

    [Put("/api/restuarant/{restuarantId}/location" + $"{ApiVersions.QueryParamVersion}")]
    Task<RestuarantBO> UpdateRestuarantLocationAsync(string restuarantId, UpdateLocationRequest request);

    [Delete("/api/restuarant/{restuarantId}" + $"{ApiVersions.QueryParamVersion}")]
    Task<RestuarantBO> RemoveRestuarantAsync(string restuarantId);

    [Get("/api/restuarant/{restuarantId}/business-hours" + $"{ApiVersions.QueryParamVersion}")]
    Task<PaginationResponse<RestuarantBO>> ListBusinessHoursAsync(FilterQueryParameters queryParameters);

    [Get("/api/restuarant/{restuarantId}/business-hours/{businessHourId}" + $"{ApiVersions.QueryParamVersion}")]
    Task<RestuarantBO?> GetBusinessHourAsync(string restuarantId, string businessHourId);

    [Post("/api/restuarant/{restuarantId}/business-hours" + $"{ApiVersions.QueryParamVersion}")]
    Task<RestuarantBO> CreateBusinessHourAsync(CreateBusinessHourRequest request);

    [Post("/api/restuarant/{restuarantId}/business-hours/bulk" + $"{ApiVersions.QueryParamVersion}")]
    Task<RestuarantBO> CreateManyBusinessHoursAsync(CreateBusinessHourRequest[] request);

    [Put("/api/restuarant/{restuarantId}/business-hours/{businessHourId}" + $"{ApiVersions.QueryParamVersion}")]
    Task<RestuarantBO> UpdateBusinessHourAsync(string restuarantId, string businessHourId, UpdateBusinessHourRequest request);

    [Delete("/api/restuarant/{restuarantId}/business-hours/{businessHourId}" + $"{ApiVersions.QueryParamVersion}")]
    Task<RestuarantBO> RemoveBusinessHourAsync(string restuarantId, string businessHourId);
}
