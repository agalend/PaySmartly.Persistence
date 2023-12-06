using Microsoft.Extensions.Options;
using PaySmartly.Persistance.Mongo;

namespace PaySmartly.Persistance.Env
{
    public interface IEnvProvider
    {
        string GetServiceName();
        PaySlipsDatabaseSettings GetDbSettings();
    }

    public class EnvProvider(IOptions<PaySlipsDatabaseSettings> bookStoreDatabaseSettings) : IEnvProvider
    {
        private readonly IOptions<PaySlipsDatabaseSettings> bookStoreDatabaseSettings = bookStoreDatabaseSettings;

        public PaySlipsDatabaseSettings GetDbSettings()
        {
            PaySlipsDatabaseSettings settings = bookStoreDatabaseSettings.Value;
            string? connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

            if (connectionString is not null)
            {
                settings = new()
                {
                    ConnectionString = connectionString,
                    DatabaseName = settings.DatabaseName,
                    PaySlipsCollectionName = settings.PaySlipsCollectionName
                };
            }

            return settings;
        }

        public string GetServiceName()
        {
            throw new NotImplementedException();
        }
    }
}