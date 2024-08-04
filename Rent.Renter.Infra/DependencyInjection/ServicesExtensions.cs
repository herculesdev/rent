using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Rent.Renter.Core.Data;
using Rent.Renter.Core.Entities;
using Rent.Renter.Infra.Data.Repositories;

namespace Rent.Renter.Infra.DependencyInjection;

public static class ServicesExtensions
{
    public static void ConfigureRenterModule(this IServiceCollection services, ConfigurationManager config, bool isWorker = false)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ServicesExtensions).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(Motorbike).Assembly);
        });
        
        services.AddSingleton<IMongoClient>(_ => new MongoClient(config.GetConnectionString("NoSqlDb")));

        if (isWorker)
        {
            services.AddTransient<IMotorbikeRepository, MotorbikeNoSqlRepository>();
        }
        else
        {
            services.AddScoped<IMotorbikeRepository, MotorbikeNoSqlRepository>();
        }
    }
}