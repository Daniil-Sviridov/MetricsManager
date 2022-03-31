using AutoMapper;
using MetricsAgent;
using MetricsAgent.DAL;
using MetricsAgent.Jobs;
using NLog;
using NLog.Web;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

using Core;

using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;

using Newtonsoft.Json;

using Microsoft.OpenApi.Models;
using System.Reflection;

NLog.GlobalDiagnosticsContext.Set("LogDirectory", Path.Combine(Directory.GetCurrentDirectory(), "Logs"));

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
var builder = WebApplication.CreateBuilder(args);

var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
var mapper = mapperConfiguration.CreateMapper();

// Add services to the container.
builder.Services.AddControllers();

// NLog: настройка NLog для внедрения зависимостей
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddEventLog();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();

logger.Debug("Приложение запущено.");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddSingleton<IRepository, NullRepository>();
builder.Services.AddSingleton<ICpuMetricsRepository, CpuMetricsRepository>();
builder.Services.AddSingleton<IDotNetMetricsRepository, DotNetMetricsRepository>();
builder.Services.AddSingleton<IHddMetricsRepository, HddMetricsRepository>();
builder.Services.AddSingleton<INetworkMetricsRepository, NetworkMetricsRepository>();
builder.Services.AddSingleton<IRamMetricsRepository, RamMetricsRepository>();


builder.Services.AddSingleton<IJobFactory, SingletonJobFactory>();
builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

// Добавляем задачи
builder.Services.AddSingleton<CpuMetricJob>();
builder.Services.AddSingleton<DotNetMetricJob>();
builder.Services.AddSingleton<HddMetricJob>();
builder.Services.AddSingleton<NetworkMetricJob>();
builder.Services.AddSingleton<RamMetricJob>();

string stringExpression = "0/5 * * * * ?"; // Запускать каждые 5 секунд

builder.Services.AddSingleton(new JobSchedule(jobType: typeof(CpuMetricJob)    , cronExpression: stringExpression));
builder.Services.AddSingleton(new JobSchedule(jobType: typeof(DotNetMetricJob) , cronExpression: stringExpression));
builder.Services.AddSingleton(new JobSchedule(jobType: typeof(HddMetricJob)    , cronExpression: stringExpression));
builder.Services.AddSingleton(new JobSchedule(jobType: typeof(NetworkMetricJob), cronExpression: stringExpression));
builder.Services.AddSingleton(new JobSchedule(jobType: typeof(RamMetricJob)    , cronExpression: stringExpression));

builder.Services.AddHostedService<QuartzHostedService>();

builder.Services.AddSingleton(mapper);

builder.Services.AddSingleton<IConnectionManager, ConnectionManager>();

/*builder.Services.AddFluentMigratorCore().ConfigureRunner(rb => rb.AddSQLite().WithGlobalConnectionString(ConnectionManager.ConnectionString)).AddLogging(lb => lb.AddFluentMigratorConsole());

var appSettings = new AppSettings();
builder.Configuration.Bind(appSettings);

string migrationAssemblyPath = Path.Combine(appSettings.ExecutingAssembly.Location.LeftOfRightmostOf("\\"), appSettings.MigrationAssembly);

Assembly migrationAssembly = Assembly.LoadFrom(migrationAssemblyPath);*/


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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
