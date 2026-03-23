namespace Demo.Restuarants.API.SDK;

public record RestuarantApiOptions
(
    string BaseUrl
)
{
    public const string ConfigKey = "RestuarantApi";

    public RestuarantApiOptions() : this(string.Empty)
    { }
}
