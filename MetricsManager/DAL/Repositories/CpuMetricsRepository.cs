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
    public interface ICpuMetricsRepository : IRepositoryMgr<CpuMetric>
    {

    }

    public class CpuMetricsRepository : ICpuMetricsRepository
    {
        private readonly IConnectionManager _connectionManager;

        public CpuMetricsRepository(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        // Инжектируем соединение с базой данных в наш репозиторий через конструктор

        public void Create(CpuMetric item)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                // Запрос на вставку данных с плейсхолдерами для параметров
                connection.Execute("INSERT INTO cpumetrics(agentid,value, time) VALUES(@agentid, @value, @time)",
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
                connection.Execute("DELETE FROM cpumetrics WHERE id=@id",
                new
                {
                    id = id
                });
            }
        }
        public void Update(CpuMetric item)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                connection.Execute("UPDATE cpumetrics SET value = @value, time = @time WHERE id = @id",
                new
                {
                    value = item.Value,
                    time = item.Time,
                    id = item.Id
                });
            }

        }

        public IList<CpuMetric> GetAll()
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                return connection.Query<CpuMetric>("SELECT id, agentid ,time, value FROM cpumetrics").ToList();
            }
        }

        public IList<CpuMetric> GetMetricsOutPeriodByAgentId(int agentId, long fromTime, long toTime)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                return connection.Query<CpuMetric>("SELECT id, agentid, value, time FROM cpumetrics WHERE time>@fromTime AND time<@toTime AND agentid = @agentid",
                new { agentid = agentId, fromTime = fromTime, toTime = toTime }).ToList();
            }
        }

        public IList<CpuMetric> GetMetricsOutPeriod(long fromTime, long toTime)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                return connection.Query<CpuMetric>("SELECT id, agentid, value, time FROM cpumetrics WHERE time>@fromTime AND time<@toTime",
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
                    max = connection.QuerySingle<long>("SELECT MAX(time) FROM cpumetrics where agentid = @agentid", new { agentid = agentid });
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
