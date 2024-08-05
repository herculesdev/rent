using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rent.Backoffice.Core.Data;
using Rent.Backoffice.Core.Entities;
using Rent.Backoffice.Infra.Data.Contexts;
using Rent.Backoffice.Infra.Data.Repositories;

namespace Rent.Backoffice.Infra.DependencyInjection;

public static class ServicesExtensions
{
    public static void ConfigureBackofficeModule(this IServiceCollection services, IConfigurationManager config)
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