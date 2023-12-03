using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PaySmartly.Persistance.Mongo;

namespace PaySmartly.Persistance.Repository
{
    public class MongoRepository : IRepository<MongoRecord>
    {
        private readonly IMongoCollection<MongoRecord> paySlipRecords;

        public MongoRepository(IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
        {
            MongoClient mongoClient = new(bookStoreDatabaseSettings.Value.ConnectionString);

            IMongoDatabase mongoDatabase = mongoClient.GetDatabase(bookStoreDatabaseSettings.Value.DatabaseName);

            paySlipRecords = mongoDatabase.GetCollection<MongoRecord>(bookStoreDatabaseSettings.Value.BooksCollectionName);
        }

        public async Task<MongoRecord?> Add(MongoRecord paySlipRecord)
        {
            await paySlipRecords.InsertOneAsync(paySlipRecord);

            MongoRecord? record = await Get(paySlipRecord.Id!);

            return record;
        }

        public async Task<MongoRecord?> Get(string id)
        {
            MongoRecord? record = await paySlipRecords.Find(x => x.Id == id).FirstOrDefaultAsync();

            return record;
        }

        public async Task<long> Delete(string id)
        {
            DeleteResult result = await paySlipRecords.DeleteOneAsync(x => x.Id == id);

            return result.DeletedCount;
        }

        public async Task<long> DeleteAll(string[] ids)
        {
            DeleteResult result = await paySlipRecords.DeleteManyAsync(record => ids.Contains(record.Id));

            return result.DeletedCount;
        }

        public async Task<IEnumerable<MongoRecord>> GetAllForEmployee(string firstName, string lastName, int limit, int offset)
        {
            IEnumerable<MongoRecord> records = await paySlipRecords
                .Find(record => record.EmployeeFirstName == firstName && record.EmployeeLastName == lastName)
                .SortByDescending(record => record.CreatedAt)
                .Skip(offset)
                .Limit(limit)
                .ToListAsync();

            return records;
        }

        public async Task<IEnumerable<MongoRecord>> GetAllForSuperRate(double from, double to, int limit, int offset)
        {
            IEnumerable<MongoRecord> records = await paySlipRecords
                .Find(record => record.SuperRate >= from && record.SuperRate <= to)
                .SortByDescending(record => record.CreatedAt)
                .Skip(offset)
                .Limit(limit)
                .ToListAsync();

            return records;
        }

        public async Task<IEnumerable<MongoRecord>> GetAllForAnnualSalary(double from, double to, int limit, int offset)
        {
            IEnumerable<MongoRecord> records = await paySlipRecords
                .Find(record => record.AnnualSalary >= from && record.AnnualSalary <= to)
                .SortByDescending(record => record.CreatedAt)
                .Skip(offset)
                .Limit(limit)
                .ToListAsync();

            return records;
        }
    }
}