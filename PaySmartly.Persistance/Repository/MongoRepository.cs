using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PaySmartly.Persistance.Mongo;

namespace PaySmartly.Persistance.Repository
{
    public interface IRepository
    {
        Task<MongoRecord> Add(MongoRecord record);
        Task<MongoRecord> Get(string id);
        Task<MongoRecord> Delete(string id);
    }

    public class MongoRepository : IRepository
    {
        private readonly IMongoCollection<MongoRecord> paySlipRecords;

        public MongoRepository(IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
        {
            MongoClient mongoClient = new(bookStoreDatabaseSettings.Value.ConnectionString);

            IMongoDatabase mongoDatabase = mongoClient.GetDatabase(bookStoreDatabaseSettings.Value.DatabaseName);

            paySlipRecords = mongoDatabase.GetCollection<MongoRecord>(bookStoreDatabaseSettings.Value.BooksCollectionName);
        }

        public async Task<MongoRecord> Add(MongoRecord paySlipRecord)
        {
            await paySlipRecords.InsertOneAsync(paySlipRecord);

            // meanwhile someone can remove the record and we are going to return null here
            // indicating that the Add method was not successful
            MongoRecord record = await Get(paySlipRecord.Id);
            return record;
        }

        public async Task<MongoRecord> Get(string? id)
        {
            MongoRecord record = await paySlipRecords.Find(x => x.Id == id).FirstOrDefaultAsync();
            return record;
        }

        public async Task<MongoRecord> Delete(string id)
        {
            MongoRecord record = await Get(id);

            if (record is null)
            {
                return default!;
            }
            else
            {
                await paySlipRecords.DeleteOneAsync(x => x.Id == id);

                // meanwhile someone can delete this record but we have already taken the value
                // and therefore it is safe to return it
                return record;
            }
        }
    }
}