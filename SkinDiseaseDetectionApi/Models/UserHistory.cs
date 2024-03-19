using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SkinDiseaseDetectionApi.Service;

public class UserHistory
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string UserId { get; set; }
    public string Image { get; set; }
    public DateTime DateCreated { get; set; }
    public string DiagnoseResult { get; set; }
    public bool DiagnoseResultAccuracy { get; set; }
}