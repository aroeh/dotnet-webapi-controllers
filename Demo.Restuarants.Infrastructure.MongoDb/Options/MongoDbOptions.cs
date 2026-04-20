namespace Demo.Restuarants.Infrastructure.MongoDb.Options;

public record MongoDbOptions
{
    public const string ConfigKey = "RestuarantApi:Settings:MongoDb";

    public string? ConnectionString { get; set; }
    public string? DatabaseName { get; set; }
}
