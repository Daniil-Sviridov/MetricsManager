using Dapper;
using MetricsAgent.Models;
using System;
using Core;
using System.Collections.Generic;
using System.Data.SQLite;

namespace MetricsAgent.DAL
{
    public interface IHddMetricsRepository : IRepository<HddMetric>
    {

    }

    public class HddMetricsRepository : IHddMetricsRepository
    {
        private readonly IConnectionManager _connectionManager;

        public HddMetricsRepository(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;

            /*const string ConnectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";
            var connection = new SQLiteConnection(ConnectionString);
            connection.Open();

            using (var command = new SQLiteCommand(connection))
            {
                // Задаём новый текст команды для выполнения
                // Удаляем таблицу с метриками, если она есть в базе данных
                command.CommandText = "DROP TABLE IF EXISTS hddmetrics";
                // Отправляем запрос в базу данных
                command.ExecuteNonQuery();


                command.CommandText = @"CREATE TABLE hddmetrics(id INTEGER PRIMARY KEY,
                    value INT, time INTEGER)";
                command.ExecuteNonQuery();
            }*/

            SqlMapper.AddTypeHandler(new TimeSpanHandler());

        }


        public void Create(HddMetric item)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                connection.Execute("INSERT INTO hddmetrics(value, time) VALUES(@value, @time)",
                new
                {
                    value = item.Value,
                    time = item.Time.TotalSeconds
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
                    time = item.Time.TotalSeconds,
                    id = item.Id
                });
            }
        }

        public IList<HddMetric> GetAll()
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                return connection.Query<HddMetric>("SELECT Id, Time, Value FROM hddmetrics").ToList();
            }
        }

        public HddMetric GetById(int id)
        {

            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                return connection.QuerySingle<HddMetric>("SELECT Id, Time, Value FROM hddmetrics WHERE id = @id", new { id = id });
            }
        }

        public IList<HddMetric> GetMetricsOutPeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                return connection.Query<HddMetric>("SELECT id, value, time FROM hddmetrics WHERE time>@fromTime AND time<@toTime",
                new { fromTime = fromTime.TotalSeconds, toTime = toTime.TotalSeconds }).ToList();
            }
        }
    }
}