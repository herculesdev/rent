﻿using Microsoft.Extensions.Configuration;
using Rent.Shared.Library.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Rent.Shared.Library.Extensions;

public static class DependencyInjectionExtensions
{
    public static void AddMessaging(this IServiceCollection services, bool isWorker = false)
    {
        if (isWorker)
        {
            services.AddTransient<IMessageDispatcher, RabbitMqMessageDispatcher>();
            services.AddTransient<IMessageConsumer, RabbitMqMessageConsumer>();
        }else {
            services.AddScoped<IMessageDispatcher, RabbitMqMessageDispatcher>();
            services.AddScoped<IMessageConsumer, RabbitMqMessageConsumer>();
        }
    }
}