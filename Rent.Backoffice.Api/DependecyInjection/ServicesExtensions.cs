using Microsoft.EntityFrameworkCore;
using Rent.Backoffice.Core.Data;
using Rent.Backoffice.Core.Entities;
using Rent.Backoffice.Infra.Data.Contexts;
using Rent.Backoffice.Infra.Data.Repositories;

namespace Rent.Backoffice.Api.DependecyInjection;

public static class ServicesExtensions
{
    public static void ConfigureModuleAll(this IServiceCollection services, ConfigurationManager config)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ServicesExtensions).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(Motorbike).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(BackofficeContext).Assembly);
        });
        
        services.AddDbContext<BackofficeContext>(opts => { opts.UseNpgsql(config.GetConnectionString("MainConnection")); });
        services.AddScoped<IMotorbikeRepository, MotorbikeRepository>();
    }
}