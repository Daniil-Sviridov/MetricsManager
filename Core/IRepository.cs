
public interface IRepository
{
}

public interface IRepository<T> where T : class
{
    IList<T> GetAll();

    IList<T> GetMetricsOutPeriod(long fromTime, long toTime);

    T GetById(int id);

    void Create(T item);

    void Update(T item);

    void Delete(int id);
}