using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PaySmartly.Persistance.Mongo
{
    public class MongoRecord
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = null!;
        public string EmployeeFirstName { get; set; } = null!;
        public string EmployeeLastName { get; set; } = null!;
        public double AnnualSalary { get; set; }
        public double SuperRate { get; set; }
        public string PayPeriod { get; set; } = null!;
        public int RoundTo { get; set; }
        public int Months { get; set; }
        public double GrossIncome { get; set; }
        public double IncomeTax { get; set; }
        public double NetIncome { get; set; }
        public double Super { get; set; }
        public string RequesterFirstName { get; set; } = null!;
        public string RequesterLastName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}