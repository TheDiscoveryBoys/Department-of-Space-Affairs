namespace IntergalacticPassportAPI.Data
{
    public interface IBaseRepository<T>
    {
        Task<T> GetById(object id);
        Task<IEnumerable<T>> GetAll();
        Task<T> Create(T model);
        Task<T> Update(T model);
        Task<bool> Delete(object id);
        Task<bool> Exists(T model);
    }
}
