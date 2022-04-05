

public interface IRepositoryMgr<T> where T : class
{
    IList<T> GetAll();

    DateTimeOffset GetMaxDate(int agentid);

    IList<T> GetMetricsOutPeriod(long fromTime, long toTime);

    IList<T> GetMetricsOutPeriodByAgentId(int agentid, long fromTime, long toTime);

    void Create(T item);

    void Update(T item);

    void Delete(int id);
}