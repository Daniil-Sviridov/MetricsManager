
using Core;
using MetricsManager.DAL;
using MetricsManager.Jobs;
using NLog;
using NLog.Web;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

NLog.GlobalDiagnosticsContext.Set("LogDirectory", Path.Combine(Directory.GetCurrentDirectory(), "Logs"));

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // ��������� ������� � ���������.
    builder.Services.AddControllers();

    // NLog: ��������� NLog ��� ��������� ������������
    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();
    builder.Logging.AddDebug();
    builder.Logging.AddEventLog();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();

    logger.Debug("���������� ��������.");

    builder.Services.AddSingleton<ICpuMetricsRepository, CpuMetricsRepository>();

    builder.Services.AddSingleton<IJobFactory, SingletonJobFactory>();
    builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

    // ��������� ������
    builder.Services.AddSingleton<CpuMetricJob>();
  

    string stringExpression = "0/5 * * * * ?"; // ��������� ������ 5 ������

    builder.Services.AddSingleton(new JobSchedule(jobType: typeof(CpuMetricJob), cronExpression: stringExpression));

    builder.Services.AddHostedService<QuartzHostedService>();

    //builder.Services.AddSingleton(mapper);

    builder.Services.AddSingleton<IConnectionManager, ConnectionManager>();


    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // ��������� �������� HTTP-��������.
    if (!app.Environment.IsDevelopment())
    {
        /* app.UseExceptionHandler("/Home/Error");
         // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
         app.UseHsts();*/

        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}