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
    public interface ICpuMetricsRepository : IRepository<CpuMetric>
    {

    }

    public class CpuMetricsRepository : ICpuMetricsRepository
    {
        private readonly IConnectionManager _connectionManager;

        //private string ConnectionString = "Data Source=metrics11.db;Version=3;Pooling=true;Max Pool Size=100;";

        public CpuMetricsRepository(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;

            /*const string ConnectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";
            var connection = new SQLiteConnection(ConnectionString);
            connection.Open();

            using (var command = new SQLiteCommand(connection))
            {
                // Задаём новый текст команды для выполнения
                // Удаляем таблицу с метриками, если она есть в базе данных
                command.CommandText = "DROP TABLE IF EXISTS cpumetrics";
                // Отправляем запрос в базу данных
                command.ExecuteNonQuery();


                command.CommandText = @"CREATE TABLE cpumetrics(id INTEGER PRIMARY KEY,
                    value INT, time INTEGER)";
                command.ExecuteNonQuery();
            }*/

            //SqlMapper.AddTypeHandler(new TimeSpanHandler());

        }

        // Инжектируем соединение с базой данных в наш репозиторий через конструктор

        public void Create(CpuMetric item)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                // Запрос на вставку данных с плейсхолдерами для параметров
                connection.Execute("INSERT INTO cpumetrics(value, time) VALUES(@value, @time)",
                // Анонимный объект с параметрами запроса
                new
                {
                    value = item.Value,
                    time = item.Time
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
                return connection.Query<CpuMetric>("SELECT Id, Time, Value FROM cpumetrics").ToList();
            }
        }

        public CpuMetric GetById(int id)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                return connection.QuerySingle<CpuMetric>("SELECT Id, Time, Value FROM cpumetrics WHERE id = @id",
                new { id = id });
            }

        }

        public IList<CpuMetric> GetMetricsOutPeriod(long fromTime, long toTime)
        {
            using (var connection = _connectionManager.CreateOpenedConnection())
            {
                return connection.Query<CpuMetric>("SELECT id, value, time FROM cpumetrics WHERE time>@fromTime AND time<@toTime",
                new { fromTime = fromTime, toTime = toTime}).ToList();
            }
        }

        public DateTimeOffset GetMaxDate(int agentid)
        {
            throw new NotImplementedException();
        }
    }
}
