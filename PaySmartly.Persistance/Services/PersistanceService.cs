using Grpc.Core;
using PaySmartly.Persistance.Mongo;
using PaySmartly.Persistance.Repository;

using static PaySmartly.Persistance.Mongo.MongoValidator;
using static PaySmartly.Persistance.Mongo.MongoResponseFactor;
using static PaySmartly.Persistance.Services.Converter;

namespace PaySmartly.Persistance.Services;

public class PersistanceService(
    IRepository<MongoRecord> repository,
    ILogger<PersistanceService> logger
    ) : Persistance.PersistanceBase
{
    private readonly IRepository<MongoRecord> repository = repository;
    private readonly ILogger<PersistanceService> logger = logger;

    public override async Task<Response> Create(CreateRequest request, ServerCallContext context)
    {
        MongoRecord? mongoRecord = Convert(request.Record);

        mongoRecord = await repository.Add(mongoRecord);

        Response response = CreateResponse(mongoRecord);

        return response;
    }

    public override async Task<Response> Get(GetRequest request, ServerCallContext context)
    {
        if (!IsValid(request.Id))
        {
            return CreateResponse(default);
        }
        else
        {

            MongoRecord? mongoRecord = await repository.Get(request.Id);

            Response response = CreateResponse(mongoRecord);
            return response;
        }
    }

    public override async Task<DeleteResponse> Delete(DeleteRequest request, ServerCallContext context)
    {
        if (!IsValid(request.Id))
        {
            return CreateDeleteResponse(default);
        }
        else
        {
            long deleted = await repository.Delete(request.Id);

            DeleteResponse response = CreateDeleteResponse(deleted);
            return response;
        }
    }

    public override async Task<DeleteResponse> DeleteAll(DeleteAllRequest request, ServerCallContext context)
    {
        if (!AreValid(request.Ids))
        {
            return CreateDeleteResponse(default);
        }
        else
        {
            long deleted = await repository.DeleteAll([.. request.Ids]);

            DeleteResponse response = CreateDeleteResponse(deleted);
            return response;
        }
    }

    public override async Task<GetAllResponse> GetAllForEmployee(GetAllForEmployeeRequest request, ServerCallContext context)
    {
        if (!IsValidGetAllForEmployeeRequest(request))
        {
            return CreateGetAllResponse(default);
        }
        else
        {
            IEnumerable<MongoRecord> result = await repository.GetAllForEmployee(request.FirstName, request.LastName, request.Limit, request.Offset);

            GetAllResponse response = CreateGetAllResponse(result);
            return response;
        }
    }

    public override async Task<GetAllResponse> GetAllForSuperRate(GetAllForSuperRateRequest request, ServerCallContext context)
    {
        if (!IsValidGetAllForSuperRateRequest(request))
        {
            return CreateGetAllResponse(default);
        }
        else
        {
            IEnumerable<MongoRecord> result = await repository.GetAllForSuperRate(request.From, request.To, request.Limit, request.Offset);

            GetAllResponse response = CreateGetAllResponse(result);
            return response;
        }
    }

    public override async Task<GetAllResponse> GetAllForAnnualSalary(GetAllForAnnualSalaryRequest request, ServerCallContext context)
    {
        if (!IsValidGetAllAnnualSalaryRequest(request))
        {
            return CreateGetAllResponse(default);
        }
        else
        {
            IEnumerable<MongoRecord> result = await repository.GetAllForAnnualSalary(request.From, request.To, request.Limit, request.Offset);

            GetAllResponse response = CreateGetAllResponse(result);
            return response;
        }
    }
}
