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
    //NLog: ������������� ����� ����������
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    // ��������� ������� 
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
            // ����� ���� ���������� � ������ ������ ����������
            catch (Exception exception)
            {
                //NLog: ������������� ����� ����������
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // ��������� ������� 
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
                logging.ClearProviders(); // �������� ����������� �����������
                logging.SetMinimumLevel(LogLevel.Trace); // ������������� ����������� ������� �����������
            }).UseNLog(); // ��������� ���������� nlog
    }

    internal class Startup
    {
        public Startup()
        {
        }
    }
}*/