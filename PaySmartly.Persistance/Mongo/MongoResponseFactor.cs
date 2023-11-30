using static PaySmartly.Persistance.Services.Converter;

namespace PaySmartly.Persistance.Mongo
{
    public static class MongoResponseFactor
    {
        public static Response CreateResponse(MongoRecord? mongoRecord)
        {
            if (mongoRecord is null)
            {
                return new() { Exists = false };
            }
            else
            {
                Record record = Convert(mongoRecord);
                return new() { Record = record, Exists = true };
            }
        }

        public static DeleteResponse CreateDeleteResponse(long? count)
        {
            return count is null
            ? new DeleteResponse() { Count = 0 }
            : new DeleteResponse() { Count = count.Value };
        }

        public static GetAllResponse CreateGetAllResponse(IEnumerable<MongoRecord>? records)
        {
            if (records?.Count() <= 0)
            {
                return new GetAllResponse() { Exists = false };
            }
            else
            {
                var converted = records?.Select(Convert) ?? Enumerable.Empty<Record>();

                GetAllResponse response = new() { Exists = true };
                response.Records.AddRange(converted);

                return response;
            }
        }
    }
}