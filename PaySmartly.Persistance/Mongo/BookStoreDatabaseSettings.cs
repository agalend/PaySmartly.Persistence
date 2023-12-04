namespace PaySmartly.Persistance.Mongo
{
    public class BookStoreDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string PaySlipsCollectionName { get; set; } = null!;
    }
}