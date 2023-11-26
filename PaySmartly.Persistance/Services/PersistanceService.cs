using System.Collections.Concurrent;
using Grpc.Core;

namespace PaySmartly.Persistance.Services;

public class PersistanceService(ILogger<PersistanceService> logger) : Persistance.PersistanceBase
{
    private readonly ILogger<PersistanceService> logger = logger;

    private static int currentId = -1;
    private static readonly ConcurrentDictionary<string, Record> records = new();

    public override Task<Record> Create(CreateRequest request, ServerCallContext context)
    {
        string id = GenerateNextId(ref currentId);

        Record record = new()
        {
            Id = id,
            Data = request.Data
        };

        Record added = records.AddOrUpdate(id, record, (key, old) => record);

        return Task.FromResult(record);
    }

    public override Task<Record?> Get(GetRequest request, ServerCallContext context)
    {
        records.TryGetValue(request.Id, out Record? record);

        // TODO: return default record with null

        return Task.FromResult(record);
    }

    public override Task<Record?> Delete(DeleteRequest request, ServerCallContext context)
    {
        records.Remove(request.Id, out Record? record);

        // TODO: return default record with null

        return Task.FromResult(record);
    }

    private static string GenerateNextId(ref int previousId)
    {
        int id = Interlocked.Increment(ref previousId);
        string strId = id.ToString();
        return strId;
    }
}
