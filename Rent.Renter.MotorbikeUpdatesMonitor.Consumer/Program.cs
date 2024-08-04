using Rent.Renter.Infra.DependencyInjection;
using Rent.Renter.MotorbikeUpdatesMonitor.Consumer;
using Rent.Shared.Library.Extensions;
using Rent.Shared.Library.Messaging;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSerilog(cfg => cfg.ReadFrom.Configuration(builder.Configuration));
builder.Services.ConfigureRenterModule(builder.Configuration, isWorker: true);
builder.Services.Configure<RabbitMqConfig>(builder.Configuration.GetSection("RabbitMq"));
builder.Services.AddMessaging(isWorker: true);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
