using Demo.Restuarants.API.Models;
using Demo.Restuarants.Shared.Models;
using Refit;

namespace Demo.Restuarants.API.SDK;

public interface IRestuarantsApi
{
    [Get("/api/restuarants" + $"{ApiVersions.QueryParamVersion}")]
    Task<PaginationResponse<RestuarantBO>> QueryRestuarantsAsync(FilterQueryParameters queryParameters);

    [Get("/api/restuarants/{restuarantId}" + $"{ApiVersions.QueryParamVersion}")]
    Task<RestuarantBO?> GetRestuarantAsync(string restuarantId);

    [Post("/api/restuarants" + $"{ApiVersions.QueryParamVersion}")]
    Task<RestuarantBO> CreateRestuarantAsync(CreateRestuarantRequest request);

    [Post("/api/restuarants/bulk" + $"{ApiVersions.QueryParamVersion}")]
    Task<TransactionResult> CreateManyRestuarantsAsync(CreateRestuarantRequest[] request);

    [Patch("/api/restuarants/{restuarantId}" + $"{ApiVersions.QueryParamVersion}")]
    Task<TransactionResult> UpdateRestuarantAsync(string restuarantId, UpdateRestuarantRequest request);

    [Patch("/api/restuarants/{restuarantId}/location" + $"{ApiVersions.QueryParamVersion}")]
    Task<TransactionResult> UpdateRestuarantLocationAsync(string restuarantId, UpdateLocationRequest request);

    [Delete("/api/restuarants/{restuarantId}" + $"{ApiVersions.QueryParamVersion}")]
    Task<bool> RemoveRestuarantAsync(string restuarantId);

    [Get("/api/restuarants/{restuarantId}/business-hours" + $"{ApiVersions.QueryParamVersion}")]
    Task<List<RestuarantBusinessHourBO>> ListBusinessHoursAsync(string restuarantId);

    [Get("/api/restuarants/{restuarantId}/business-hours/{businessHourId}" + $"{ApiVersions.QueryParamVersion}")]
    Task<RestuarantBusinessHourBO?> GetBusinessHourAsync(string restuarantId, string businessHourId);

    [Post("/api/restuarants/{restuarantId}/business-hours" + $"{ApiVersions.QueryParamVersion}")]
    Task<RestuarantBusinessHourBO> CreateBusinessHourAsync(string restuarantId, CreateBusinessHourRequest request);

    [Post("/api/restuarants/{restuarantId}/business-hours/bulk" + $"{ApiVersions.QueryParamVersion}")]
    Task<List<RestuarantBusinessHourBO>> CreateManyBusinessHoursAsync(string restuarantId, CreateBusinessHourRequest[] request);

    [Patch("/api/restuarants/{restuarantId}/business-hours/{businessHourId}" + $"{ApiVersions.QueryParamVersion}")]
    Task<TransactionResult> UpdateBusinessHourAsync(string restuarantId, string businessHourId, UpdateBusinessHourRequest request);

    [Delete("/api/restuarants/{restuarantId}/business-hours/{businessHourId}" + $"{ApiVersions.QueryParamVersion}")]
    Task<TransactionResult> RemoveBusinessHourAsync(string restuarantId, string businessHourId);
}
