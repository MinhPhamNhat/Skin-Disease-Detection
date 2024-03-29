using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SkinDiseaseDetectionApi.Models
{
    public class Doctor
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Avartar { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}