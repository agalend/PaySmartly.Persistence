using PaySmartly.Persistance.Mongo;

namespace PaySmartly.Persistance.Repository
{
    public interface IRepository<T>
    {
        Task<T?> Add(T record);
        Task<T?> Get(string id);
        Task<long> Delete(string id);
        Task<long> DeleteAll(string[] ids);

        Task<IEnumerable<T>> GetAllForEmployee(string firstName, string lastName, int limit, int offset);
        Task<IEnumerable<T>> GetAllForSuperRate(double from, double to, int limit, int offset);
        Task<IEnumerable<T>> GetAllForAnnualSalary(double from, double to, int limit, int offset);
    }
}