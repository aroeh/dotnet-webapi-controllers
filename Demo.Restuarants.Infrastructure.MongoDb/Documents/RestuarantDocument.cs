using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Demo.Restuarants.Infrastructure.MongoDb.Documents;

public record RestuarantDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("cuisineType")]
    public string CuisineType { get; set; } = string.Empty;

    [BsonElement("website")]
    public string? Website { get; set; }

    [BsonElement("phone")]
    public string Phone { get; set; } = string.Empty;

    [BsonElement("address")]
    public LocationDocument Address { get; set; } = new();
}
