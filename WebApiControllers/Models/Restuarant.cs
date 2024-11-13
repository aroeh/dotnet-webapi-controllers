using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace WebApiControllers.Models;

/// <summary>
/// Here we've got a sample model for outlining basic information tracking a restuarant
/// the model could be made more complex, but for demonstration purposes, we're keeping it simple
/// </summary>
public record Restuarant
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("name")]
    [Required]
    public string Name { get; set; } = string.Empty;

    [BsonElement("cuisineType")]
    [Required]
    public string CuisineType { get; set; } = string.Empty;

    [BsonElement("website")]
    public Uri? Website { get; set; }

    [BsonElement("phone")]
    [Phone]
    public string Phone { get; set; } = string.Empty;

    [BsonElement("address")]
    public Location Address { get; set; } = new Location();
}
