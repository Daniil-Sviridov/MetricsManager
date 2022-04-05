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
    public interface IRamMetricsRepository : IRepositoryMgr<RamMetric>
    {

    }

    public class RamMetricsRepository : IRamMetricsRepository
    {
        private readonly IConnectionManager _connectionManager;

        public RamMetricsRepository(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        // Инжектируем соединение с базой данных в наш репозиторий через конструктор

        public void Create(RamMetric item)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                // Запрос на вставку данных с плейсхолдерами для параметров
                connection.Execute("INSERT INTO rammetrics(agentid,value, time) VALUES(@agentid, @value, @time)",
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
                connection.Execute("DELETE FROM rammetrics WHERE id=@id",
                new
                {
                    id = id
                });
            }
        }
        public void Update(RamMetric item)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                connection.Execute("UPDATE rammetrics SET value = @value, time = @time WHERE id = @id",
                new
                {
                    value = item.Value,
                    time = item.Time,
                    id = item.Id
                });
            }

        }

        public IList<RamMetric> GetAll()
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                return connection.Query<RamMetric>("SELECT id, agentid ,time, value FROM rammetrics").ToList();
            }
        }

        public IList<RamMetric> GetMetricsOutPeriodByAgentId(int agentId, long fromTime, long toTime)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                return connection.Query<RamMetric>("SELECT id, agentid, value, time FROM rammetrics WHERE time>@fromTime AND time<@toTime AND agentid = @agentid",
                new { agentid = agentId, fromTime = fromTime, toTime = toTime }).ToList();
            }
        }

        public IList<RamMetric> GetMetricsOutPeriod(long fromTime, long toTime)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                return connection.Query<RamMetric>("SELECT id, agentid, value, time FROM rammetrics WHERE time>@fromTime AND time<@toTime",
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
                    max = connection.QuerySingle<long>("SELECT MAX(time) FROM rammetrics where agentid = @agentid", new { agentid = agentid });
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
