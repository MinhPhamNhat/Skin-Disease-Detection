using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SkinDiseaseDetectionApi.Models
{
    public class UserDiseaseInformation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string DiseaseName { get; set; }
        public DateTime DateOfDiagnosis { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
    }
}