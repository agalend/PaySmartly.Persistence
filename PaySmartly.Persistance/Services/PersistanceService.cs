using Grpc.Core;
using MongoDB.Bson;
using PaySmartly.Persistance.Mongo;
using PaySmartly.Persistance.Repository;
using static PaySmartly.Persistance.Services.Converter;

namespace PaySmartly.Persistance.Services;

public class PersistanceService(
    IRepository repository,
    ILogger<PersistanceService> logger
    ) : Persistance.PersistanceBase
{
    private readonly IRepository repository = repository;
    private readonly ILogger<PersistanceService> logger = logger;

    public override async Task<Response> Create(CreateRequest request, ServerCallContext context)
    {
        MongoRecord mongoRecord = Convert(request.Record);

        mongoRecord = await repository.Add(mongoRecord);

        Response response = CreateResponse(mongoRecord);

        return response;
    }

    public override async Task<Response> Get(GetRequest request, ServerCallContext context)
    {
        if (!IsValidId(request.Id))
        {
            return CreateResponse(default);
        }

        MongoRecord mongoRecord = await repository.Get(request.Id);

        Response response = CreateResponse(mongoRecord);

        return response;
    }

    public override async Task<Response> Delete(DeleteRequest request, ServerCallContext context)
    {
        if (!IsValidId(request.Id))
        {
            return CreateResponse(default);
        }

        MongoRecord mongoRecord = await repository.Delete(request.Id);

        Response response = CreateResponse(mongoRecord);

        return response;
    }

    private bool IsValidId(string? id)
    {
        return ObjectId.TryParse(id, out _);
    }

    private Response CreateResponse(MongoRecord? mongoRecord)
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
}
