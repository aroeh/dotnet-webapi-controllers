namespace Demo.Restuarants.API.Models;

public static class ApiVersions
{
    public const string Latest = "3.0";
    public const string QueryParamVersion = "?api-version=3.0";

    public const string V1 = "1.0";
    public const string V2 = "2.0";
    public const string V3 = "3.0";

    public static string[] AllVersions => [
        "1.0",
        "2.0",
        "3.0"
    ];
}
