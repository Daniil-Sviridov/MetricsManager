using Dapper;
using MetricsAgent.Models;
using System;
using Core;
using System.Collections.Generic;
using System.Data.SQLite;

namespace MetricsAgent.DAL
{
    // Маркировочный интерфейс
    // используется, чтобы проверять работу репозитория на тесте-заглушке
    public interface IRamMetricsRepository : IRepository<RamMetric>
    {

    }

    public class RamMetricsRepository : IRamMetricsRepository
    {
        private readonly IConnectionManager _connectionManager;

        public RamMetricsRepository(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;

            /*const string ConnectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";
            var connection = new SQLiteConnection(ConnectionString);
            connection.Open();

            using (var command = new SQLiteCommand(connection))
            {
                // Задаём новый текст команды для выполнения
                // Удаляем таблицу с метриками, если она есть в базе данных
                command.CommandText = "DROP TABLE IF EXISTS rammetrics";
                // Отправляем запрос в базу данных
                command.ExecuteNonQuery();


                command.CommandText = @"CREATE TABLE rammetrics(id INTEGER PRIMARY KEY,
                    value INT, time INTEGER)";
                command.ExecuteNonQuery();
            }*/

            SqlMapper.AddTypeHandler(new TimeSpanHandler());
        }


        public void Create(RamMetric item)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                connection.Execute("INSERT INTO rammetrics(value, time) VALUES(@value, @time)",
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
                    time = item.Time.TotalSeconds,
                    id = item.Id
                });
            }
        }

        public IList<RamMetric> GetAll()
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                return connection.Query<RamMetric>("SELECT Id, Time, Value FROM rammetrics").ToList();
            }
        }

        public RamMetric GetById(int id)
        {

            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                return connection.QuerySingle<RamMetric>("SELECT Id, Time, Value FROM rammetrics WHERE id = @id", new { id = id });
            }
        }

        public IList<RamMetric> GetMetricsOutPeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                return connection.Query<RamMetric>("SELECT id, value, time FROM rammetrics WHERE time>@fromTime AND time<@toTime",
                new { fromTime = fromTime.TotalSeconds, toTime = toTime.TotalSeconds }).ToList();
            }
        }
    }
}