using Google.Protobuf.WellKnownTypes;
using PaySmartly.Persistance.Mongo;

namespace PaySmartly.Persistance.Services
{
    public static class Converter
    {
        public static Record Convert(MongoRecord record)
        {
            return new()
            {
                Id = record.Id,
                EmployeeFirstName = record.EmployeeFirstName,
                EmployeeLastName = record.EmployeeLastName,
                AnnualSalary = record.AnnualSalary,
                SuperRate = record.SuperRate,
                PayPeriod = record.PayPeriod.ToTimestamp(),
                RoundTo = record.RoundTo,
                Months = record.Months,
                GrossIncome = record.GrossIncome,
                IncomeTax = record.IncomeTax,
                NetIncome = record.NetIncome,
                Super = record.Super,
                RequesterFirstName = record.RequesterFirstName,
                RequesterLastName = record.RequesterLastName
            };
        }

        public static MongoRecord Convert(Record record)
        {
            return new()
            {
                EmployeeFirstName = record.EmployeeFirstName,
                EmployeeLastName = record.EmployeeLastName,
                AnnualSalary = record.AnnualSalary,
                SuperRate = record.SuperRate,
                PayPeriod = record.PayPeriod.ToDateTime(),
                RoundTo = record.RoundTo,
                Months = record.Months,
                GrossIncome = record.GrossIncome,
                IncomeTax = record.IncomeTax,
                NetIncome = record.NetIncome,
                Super = record.Super,
                RequesterFirstName = record.RequesterFirstName,
                RequesterLastName = record.RequesterLastName,
                CreatedAt = record.CreatedAt.ToDateTime()
            };
        }
    }
}