using MongoDB.Bson;

namespace PaySmartly.Persistance.Mongo
{
    public static class MongoValidator
    {
        public static bool IsValid(string? id)
        {
            return !string.IsNullOrEmpty(id) && ObjectId.TryParse(id, out _);
        }

        public static bool AreValid(IEnumerable<string>? ids)
        {
            return !ids?.Any(id => !IsValid(id)) ?? false;
        }

        public static bool IsValidGetAllForEmployeeRequest(GetAllForEmployeeRequest? request)
        {
            return
                !string.IsNullOrEmpty(request?.FirstName) &&
                !string.IsNullOrEmpty(request?.LastName) &&
                request?.Limit > 0;
        }

        public static bool IsValidGetAllForSuperRateRequest(GetAllForSuperRateRequest? request)
        {
            return (request?.From <= request?.To) && request?.Limit > 0;
        }

        public static bool IsValidGetAllAnnualSalaryRequest(GetAllForAnnualSalaryRequest? request)
        {
            return (request?.From <= request?.To) && request?.Limit > 0;
        }
    }
}