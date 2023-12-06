namespace PaySmartly.Persistance.Mongo
{
    public class PaySlipsDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string PaySlipsCollectionName { get; set; } = null!;
    }
}