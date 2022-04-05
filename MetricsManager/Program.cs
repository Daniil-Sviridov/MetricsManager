
using Core;
using FluentMigrator.Runner;
using MetricsManager.Client;
using MetricsManager.DAL;
using MetricsManager.DAL.Migrations;
using MetricsManager.DAL.Repositories;
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

    // Добавляем сервисы в контейнер.
    builder.Services.AddControllers();

    // NLog: настройка NLog для внедрения зависимостей
    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();
    builder.Logging.AddDebug();
    builder.Logging.AddEventLog();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();

    logger.Debug("Приложение запущено.");

    builder.Services.AddSingleton<IAgentsRepository, AgentsRepository>();
    builder.Services.AddSingleton<ICpuMetricsRepository, CpuMetricsRepository>();
    builder.Services.AddSingleton<IDotNetMetricsRepository, DotNetMetricsRepository>();
    builder.Services.AddSingleton<INetworkMetricsRepository, NetworkMetricsRepository>();
    builder.Services.AddSingleton<IHddMetricsRepository, HddMetricsRepository>();
    builder.Services.AddSingleton<IRamMetricsRepository, RamMetricsRepository>();

    builder.Services.AddSingleton<IJobFactory, SingletonJobFactory>();
    builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

    // Добавляем задачи
    builder.Services.AddSingleton<CpuMetricJob>();
    builder.Services.AddSingleton<DotNetMetricJob>();
    builder.Services.AddSingleton<NetworkMetricJob>();
    builder.Services.AddSingleton<HddMetricJob>();
    builder.Services.AddSingleton<RamMetricJob>();


    string stringExpression = "0/50 * * * * ?"; // Запускать каждые 5 секунд

    builder.Services.AddSingleton(new JobSchedule(jobType: typeof(CpuMetricJob), cronExpression: stringExpression));
    builder.Services.AddSingleton(new JobSchedule(jobType: typeof(DotNetMetricJob), cronExpression: stringExpression));
    builder.Services.AddSingleton(new JobSchedule(jobType: typeof(NetworkMetricJob), cronExpression: stringExpression));
    builder.Services.AddSingleton(new JobSchedule(jobType: typeof(HddMetricJob), cronExpression: stringExpression));
    builder.Services.AddSingleton(new JobSchedule(jobType: typeof(RamMetricJob), cronExpression: stringExpression));

    builder.Services.AddHostedService<QuartzHostedService>();

    //builder.Services.AddSingleton(mapper);

    builder.Services.AddSingleton<IConnectionManager, ConnectionManager>();

    builder.Services.AddHttpClient<IMetricsAgentClient, MetricsAgentClient>();
    //.AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _=> TimeSpan.FromMilliseconds(1000)))


    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var serviceProvider = new ServiceCollection()
        // Add common FluentMigrator services
        .AddFluentMigratorCore()
        .ConfigureRunner(rb => rb
            // Add SQLite support to FluentMigrator
            .AddSQLite()
            // Set the connection string
            .WithGlobalConnectionString(ConnectionManager.ConnectionString)
            // Define the assembly containing the migrations
            .ScanIn(typeof(FirstMigration).Assembly).For.Migrations())
        // Enable logging to console in the FluentMigrator way
        .AddLogging(lb => lb.AddFluentMigratorConsole())
        // Build the service provider
        .BuildServiceProvider(false);

    // Put the database update into a scope to ensure
    // that all resources will be disposed.
    using (var scope = serviceProvider.CreateScope())
    {
        // Instantiate the runner
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

        // Execute the migrations
        runner.MigrateUp();
    }

    var app = builder.Build();

    // Настройте конвейер HTTP-запросов.
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