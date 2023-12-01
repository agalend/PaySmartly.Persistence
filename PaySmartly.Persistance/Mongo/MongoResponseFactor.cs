using static PaySmartly.Persistance.Services.Converter;

namespace PaySmartly.Persistance.Mongo
{
    public static class MongoResponseFactor
    {
        public static Response CreateResponse(bool invalidParameters, MongoRecord? mongoRecord)
        {
            if (invalidParameters)
            {
                return new() { Exists = false, InvalidParameters = invalidParameters };
            }
            else
            {
                Record? record = mongoRecord is null ? default : Convert(mongoRecord);
                return new() { Record = record, Exists = record is not null, InvalidParameters = invalidParameters };
            }
        }

        public static DeleteResponse CreateDeleteResponse(bool invalidParameters, long? count)
        {
            return invalidParameters
            ? new DeleteResponse() { Count = 0, InvalidParameters = invalidParameters }
            : new DeleteResponse() { Count = count ?? 0, InvalidParameters = invalidParameters };
        }

        public static GetAllResponse CreateGetAllResponse(bool invalidParameters, IEnumerable<MongoRecord>? records)
        {
            if (invalidParameters)
            {
                return new GetAllResponse() { Exists = false, InvalidParameters = invalidParameters };
            }
            else
            {
                var converted = records?.Select(Convert) ?? Enumerable.Empty<Record>();

                GetAllResponse response = new() { Exists = converted.Any(), InvalidParameters = invalidParameters };
                response.Records.AddRange(converted);

                return response;
            }
        }
    }
}