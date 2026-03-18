namespace Demo.Restuarants.API.MongoDb.Infrastructure.Options;

public record MongoDbOptions
{
    public const string ConfigKey = "MongoDb";

    public string? ConnectionString { get; set; }
    public string? DatabaseName { get; set; }
}
