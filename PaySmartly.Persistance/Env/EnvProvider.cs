using Microsoft.Extensions.Options;
using PaySmartly.Persistance.Mongo;

namespace PaySmartly.Persistance.Env
{
    public interface IEnvProvider
    {
        string GetServiceName();
        PaySlipsDatabaseSettings GetDbSettings();
    }

    public class EnvProvider(IOptions<PaySlipsDatabaseSettings> paySlipsDatabaseSettings) : IEnvProvider
    {
        private readonly PaySlipsDatabaseSettings defaultSettings = paySlipsDatabaseSettings.Value;

        public PaySlipsDatabaseSettings GetDbSettings()
        {
            string? connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            PaySlipsDatabaseSettings settings = defaultSettings;

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