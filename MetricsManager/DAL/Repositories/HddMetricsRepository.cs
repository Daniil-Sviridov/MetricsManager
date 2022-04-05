using Dapper;
using System;
using Core;
using System.Collections.Generic;
using System.Data.SQLite;
using MetricsManager.Models;

namespace MetricsManager.DAL.Repositories
{
    // Маркировочный интерфейс
    // используется, чтобы проверять работу репозитория на тесте-заглушке
    public interface IHddMetricsRepository : IRepositoryMgr<HddMetric>
    {

    }

    public class HddMetricsRepository : IHddMetricsRepository
    {
        private readonly IConnectionManager _connectionManager;

        public HddMetricsRepository(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        // Инжектируем соединение с базой данных в наш репозиторий через конструктор

        public void Create(HddMetric item)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                // Запрос на вставку данных с плейсхолдерами для параметров
                connection.Execute("INSERT INTO hddmetrics(agentid,value, time) VALUES(@agentid, @value, @time)",
                // Анонимный объект с параметрами запроса
                new
                {
                    agentid = item.AgentId,
                    value = item.Value,
                    time = item.Time,
                });
            }
        }

        public void Delete(int id)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                connection.Execute("DELETE FROM hddmetrics WHERE id=@id",
                new
                {
                    id = id
                });
            }
        }
        public void Update(HddMetric item)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                connection.Execute("UPDATE hddmetrics SET value = @value, time = @time WHERE id = @id",
                new
                {
                    value = item.Value,
                    time = item.Time,
                    id = item.Id
                });
            }

        }

        public IList<HddMetric> GetAll()
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                return connection.Query<HddMetric>("SELECT id, agentid ,time, value FROM hddmetrics").ToList();
            }
        }

        public IList<HddMetric> GetMetricsOutPeriodByAgentId(int agentId, long fromTime, long toTime)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                return connection.Query<HddMetric>("SELECT id, agentid, value, time FROM hddmetrics WHERE time>@fromTime AND time<@toTime AND agentid = @agentid",
                new { agentid = agentId, fromTime = fromTime, toTime = toTime }).ToList();
            }
        }

        public IList<HddMetric> GetMetricsOutPeriod(long fromTime, long toTime)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                return connection.Query<HddMetric>("SELECT id, agentid, value, time FROM hddmetrics WHERE time>@fromTime AND time<@toTime",
                new { fromTime = fromTime, toTime = toTime }).ToList();
            }
        }

        public DateTimeOffset GetMaxDate(int agentid)
        {
            long max = 0;

            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                try
                {
                    max = connection.QuerySingle<long>("SELECT MAX(time) FROM hddmetrics where agentid = @agentid", new { agentid = agentid });
                }
                catch (Exception ex)
                {
                    //_logger.
                }

                return DateTimeOffset.FromUnixTimeSeconds(max).DateTime;
            }
        }
    }
}
