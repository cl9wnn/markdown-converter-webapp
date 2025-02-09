using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Persistence.Entities;

public class ChangeRecordEntity
{
    [BsonId]
    public ObjectId Id { get; set; } 

    [BsonRepresentation(BsonType.String)]
    public Guid DocumentId { get; set; }

    [BsonRepresentation(BsonType.String)]
    public Guid? AccountId { get; set; }

    public DateTime ChangeDate { get; set; }
}