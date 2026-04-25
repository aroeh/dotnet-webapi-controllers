namespace Demo.Restuarants.API.Models;

public static class ApiVersions
{
    public const string Latest = V1;
    public const string QueryParamVersion = $"?api-version={Latest}";

    public const string V1 = "1.0";

    public static string[] AllVersions => [
        V1
    ];
}
