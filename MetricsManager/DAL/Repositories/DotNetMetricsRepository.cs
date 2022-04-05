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
    public interface IDotNetMetricsRepository : IRepositoryMgr<DotNetMetric>
    {

    }

    public class DotNetMetricsRepository : IDotNetMetricsRepository
    {
        private readonly IConnectionManager _connectionManager;

        public DotNetMetricsRepository(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        // Инжектируем соединение с базой данных в наш репозиторий через конструктор

        public void Create(DotNetMetric item)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                // Запрос на вставку данных с плейсхолдерами для параметров
                connection.Execute("INSERT INTO dotnetmetrics(agentid,value, time) VALUES(@agentid, @value, @time)",
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
                connection.Execute("DELETE FROM dotnetmetrics WHERE id=@id",
                new
                {
                    id = id
                });
            }
        }
        public void Update(DotNetMetric item)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                connection.Execute("UPDATE dotnetmetrics SET value = @value, time = @time WHERE id = @id",
                new
                {
                    value = item.Value,
                    time = item.Time,
                    id = item.Id
                });
            }

        }

        public IList<DotNetMetric> GetAll()
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                return connection.Query<DotNetMetric>("SELECT id, agentid ,time, value FROM dotnetmetrics").ToList();
            }
        }

        public IList<DotNetMetric> GetMetricsOutPeriodByAgentId(int agentId, long fromTime, long toTime)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                return connection.Query<DotNetMetric>("SELECT id, agentid, value, time FROM dotnetmetrics WHERE time>@fromTime AND time<@toTime AND agentid = @agentid",
                new { agentid = agentId, fromTime = fromTime, toTime = toTime }).ToList();
            }
        }

        public IList<DotNetMetric> GetMetricsOutPeriod(long fromTime, long toTime)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                return connection.Query<DotNetMetric>("SELECT id, agentid, value, time FROM dotnetmetrics WHERE time>@fromTime AND time<@toTime",
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
                    max = connection.QuerySingle<long>("SELECT MAX(time) FROM dotnetmetrics where agentid = @agentid", new { agentid = agentid });
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
