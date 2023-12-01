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

        public static bool IsValidCreateRequest(CreateRequest? request)
        {
            Record? record = request?.Record;

            return
                !string.IsNullOrEmpty(record?.EmployeeFirstName) &&
                !string.IsNullOrEmpty(record?.EmployeeLastName) &&
                record?.AnnualSalary is not null &&
                record?.SuperRate is not null &&
                record?.PayPeriodFrom is not null && record?.PayPeriodFrom != default &&
                record?.PayPeriodTo is not null && record?.PayPeriodTo != default &&
                record?.RoundTo is not null &&
                record?.Months is not null &&
                record?.GrossIncome is not null &&
                record?.IncomeTax is not null &&
                record?.NetIncome is not null &&
                record?.Super is not null &&
                !string.IsNullOrEmpty(record?.RequesterFirstName) &&
                !string.IsNullOrEmpty(record?.RequesterFirstName) &&
                record?.CreatedAt is not null && record?.CreatedAt != default;
        }
    }
}