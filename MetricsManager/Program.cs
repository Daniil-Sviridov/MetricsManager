using NLog.Web;

var builder = WebApplication.CreateBuilder();

var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

try
{
    logger.Debug("init main");

    var app = builder.Build();

    builder.Services.AddSingleton<ILogger>();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.UseEndpoints(endpoints => endpoints.MapControllers());

    app.Run();
}
catch (Exception ex)
{
    //NLog: устанавливаем отлов исключений
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    // Остановка логгера 
    NLog.LogManager.Shutdown();
}

/*
using NLog.Web;

namespace MetricsManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                CreateHostBuilder(args).Build().Run();
            }
            // Отлов всех исключений в рамках работы приложения
            catch (Exception exception)
            {
                //NLog: устанавливаем отлов исключений
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Остановка логгера 
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
            {
                _ = webBuilder.UseStartup<Startup>();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders(); // Создание провайдеров логирования
                logging.SetMinimumLevel(LogLevel.Trace); // Устанавливаем минимальный уровень логирования
            }).UseNLog(); // Добавляем библиотеку nlog
    }

    internal class Startup
    {
        public Startup()
        {
        }
    }
}*/