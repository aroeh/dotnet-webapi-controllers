namespace Demo.Restuarants.Infrastructure.Azure.Models;

/// <summary>
/// Represents a binding class for Azure App Config Settings which can be used through
/// the application
/// </summary>
/// <remarks>
/// This class is optional and enables using the settings through the app via dependency injection.
/// Use <see cref="IOptions<Settings>"/>
/// </remarks>
public record Settings
{
    public string? HealthEndpoint { get; set; }
}
