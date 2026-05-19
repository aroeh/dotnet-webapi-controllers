using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Demo.Restuarants.Infrastructure.MongoDb.Documents;

public record BusinessHourDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("dayOfWeek")]
    public string DayOfWeek { get; set; } = string.Empty;

    [BsonElement("openTime")]
    public TimeOnly OpenTime { get; set; }

    [BsonElement("closeTime")]
    public TimeOnly CloseTime { get; set; }
}
