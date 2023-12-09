using Microsoft.Extensions.Options;
using PaySmartly.Persistance.Mongo;

namespace PaySmartly.Persistance.Env
{
    public interface IEnvProvider
    {
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
    }
}