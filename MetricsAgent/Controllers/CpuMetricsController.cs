using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SQLite;

namespace MetricsAgent.Controllers
{
    public class CpuMetric
    {
        public int Id { get; set; }

        public int Value { get; set; }

        public long Time { get; set; }
    }

    [Route("api/metrics/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        IRepository _repository;
        private readonly ILogger<CpuMetricsController> _logger;

        public CpuMetricsController(ILogger<CpuMetricsController> logger, IRepository repository)
        {
            _repository = repository;
            _logger = logger;

            _logger.LogInformation("NLog встроен в CpuMetricsController");
        }

        [HttpGet("sql-test")]
        public IActionResult TryToSqlLite()
        {
            string cs = "Data Source=:memory:";
            string stm = "SELECT SQLITE_VERSION()";


            using (var con = new SQLiteConnection(cs))
            {
                con.Open();

                using var cmd = new SQLiteCommand(stm, con);
                string version = cmd.ExecuteScalar().ToString();

                return Ok(version);
            }
        }

        [HttpGet("sql-read-write-test")]
        public IActionResult TryToInsertAndRead()
        {
            // Создаём строку подключения в виде базы данных в оперативной памяти
            string connectionString = "Data Source=:memory:";

            // Создаём соединение с базой данных
            using (var connection = new SQLiteConnection(connectionString))
            {
                // Открываем соединение
                connection.Open();

                // Создаём объект, через который будут выполняться команды к базе данных
                using (var command = new SQLiteCommand(connection))
                {
                    // Задаём новый текст команды для выполнения
                    // Удаляем таблицу с метриками, если она есть в базе данных
                    /*command.CommandText = "DROP TABLE IF EXISTS cpumetrics";
                    // Отправляем запрос в базу данных
                    command.ExecuteNonQuery();*/

                    // Создаём таблицу с метриками
                    command.CommandText = @"CREATE TABLE cpumetrics(id INTEGER PRIMARY KEY,
                    value INT, time INT)";
                    command.ExecuteNonQuery();

                    // Создаём запрос на вставку данных
                    command.CommandText = "INSERT INTO cpumetrics(value, time) VALUES(10,1)";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT INTO cpumetrics(value, time) VALUES(50,2)";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT INTO cpumetrics(value, time) VALUES(75,4)";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT INTO cpumetrics(value, time) VALUES(90,5)";
                    command.ExecuteNonQuery();

                    // Создаём строку для выборки данных из базы
                    // LIMIT 3 обозначает, что мы достанем только 3 записи
                    string readQuery = "SELECT * FROM cpumetrics ";

                    // Создаём массив, в который запишем объекты с данными из базы данных
                    var returnArray = new CpuMetric[4];
                    // Изменяем текст команды на наш запрос чтения
                    command.CommandText = readQuery;

                    // Создаём читалку из базы данных
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        // Счётчик, чтобы записать объект в правильное место в массиве
                        var counter = 0;
                        // Цикл будет выполняться до тех пор, пока есть что читать из базы данных
                        while (reader.Read())
                        {
                            // Создаём объект и записываем его в массив
                            returnArray[counter] = new CpuMetric
                            {
                                Id = reader.GetInt32(0), // Читаем данные, полученные из базы данных
                                Value = reader.GetInt32(1), // преобразуя к целочисленному типу
                                Time = reader.GetInt64(2)
                            };
                            // Увеличиваем значение счётчика
                            counter++;
                        }
                    }
                    // Оборачиваем массив с данными в объект ответа и возвращаем пользователю 
                    return Ok(returnArray);
                }
            }
        }


        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            return Ok();
        }
    }
}
