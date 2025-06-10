namespace Repositories;

public interface IRepository<T>
{
    List<T> GetAll();
    T GetById(Guid id);  
    void Add(T item);
    void Add(IEnumerable<T> items);
    void Update(T item);
    void Delete(Guid id);
}  