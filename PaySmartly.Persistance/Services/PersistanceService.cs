using Grpc.Core;
using PaySmartly.Persistance.Mongo;
using PaySmartly.Persistance.Repository;

namespace PaySmartly.Persistance.Services;

public class PersistanceService(
    IRepository repository,
    ILogger<PersistanceService> logger
    ) : Persistance.PersistanceBase
{
    private readonly IRepository repository = repository;
    private readonly ILogger<PersistanceService> logger = logger;

    public override async Task<Record> Create(CreateRequest request, ServerCallContext context)
    {
        MongoRecord mongoRecord = Convert(request.Data);

        mongoRecord = await repository.Add(mongoRecord);

        if (mongoRecord is null)
        {
            return new() { Exists = false };
        }
        else
        {
            Record paySlipRecord = Convert(mongoRecord);
            return paySlipRecord;
        }
    }

    public override async Task<Record?> Get(GetRequest request, ServerCallContext context)
    {
        MongoRecord mongoRecord = await repository.Get(request.Id);

        if (mongoRecord is null)
        {
            return new() { Exists = false };
        }
        else
        {
            Record paySlipRecord = Convert(mongoRecord);
            return paySlipRecord;
        }
    }

    public override async Task<Record?> Delete(DeleteRequest request, ServerCallContext context)
    {
        MongoRecord mongoRecord = await repository.Delete(request.Id);

        if (mongoRecord is null)
        {
            return new() { Exists = false };
        }
        else
        {

            Record paySlipRecord = Convert(mongoRecord);
            return paySlipRecord;
        }
    }

    private Record Convert(MongoRecord record)
    {
        return new()
        {
            Id = record.Id,
            Data = new Data()
            {
                EmployeeFirstName = record.EmployeeFirstName,
                EmployeeLastName = record.EmployeeLastName,
                AnnualSalary = record.AnnualSalary,
                SuperRate = record.SuperRate,
                PayPeriod = record.PayPeriod,
                RoundTo = record.RoundTo,
                Months = record.Months,
                GrossIncome = record.GrossIncome,
                IncomeTax = record.IncomeTax,
                NetIncome = record.NetIncome,
                Super = record.Super,
                RequesterFirstName = record.RequesterFirstName,
                RequesterLastName = record.RequesterLastName
            },
            Exists = true
        };
    }

    public MongoRecord Convert(Data data)
    {
        return new()
        {
            EmployeeFirstName = data.EmployeeFirstName,
            EmployeeLastName = data.EmployeeLastName,
            AnnualSalary = data.AnnualSalary,
            SuperRate = data.SuperRate,
            PayPeriod = data.PayPeriod,
            RoundTo = data.RoundTo,
            Months = data.Months,
            GrossIncome = data.GrossIncome,
            IncomeTax = data.IncomeTax,
            NetIncome = data.NetIncome,
            Super = data.Super,
            RequesterFirstName = data.RequesterFirstName,
            RequesterLastName = data.RequesterLastName
        };
    }
}
