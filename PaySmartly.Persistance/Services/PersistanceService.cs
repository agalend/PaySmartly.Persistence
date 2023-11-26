using System.Collections.Concurrent;
using Grpc.Core;

namespace PaySmartly.Persistance.Services;

public class PersistanceService(ILogger<PersistanceService> logger) : Persistance.PersistanceBase
{
    private readonly ILogger<PersistanceService> logger = logger;

    private int currentId = -1;
    private readonly ConcurrentDictionary<string, Record> records = new();

    public override Task<Record> Create(CreateRequest request, ServerCallContext context)
    {
        string id = GenerateNextId(ref currentId);

        Record record = new()
        {
            Id = id,
            PaySlip = request.PaySlip
        };

        Record added = records.AddOrUpdate(id, record, (key, old) => record);

        return Task.FromResult(record);
    }

    public override Task<Record?> Get(GetRequest request, ServerCallContext context)
    {
        records.TryGetValue(request.Id, out Record? record);

        return Task.FromResult(record);
    }

    public override Task<Record?> Delete(DeleteRequest request, ServerCallContext context)
    {
        records.Remove(request.Id, out Record? record);

        return Task.FromResult(record);
    }

    private static string GenerateNextId(ref int previousId)
    {
        int id = Interlocked.Increment(ref previousId);
        string strId = id.ToString();
        return strId;
    }
}
