﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NeerCore.DependencyInjection.Extensions;
using NetHub.Application.Options;
using NetHub.Application.SharedServices;
using NetHub.Core.Constants;
using NetHub.Infrastructure.Services.Internal.Sieve;
using Ng.Services;
using Sieve.Models;
using Sieve.Services;

namespace NetHub.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAllServices(options => options.ResolveInternalImplementations = true);
        services.AddLazyCache();
        services.AddCustomSieve(configuration);
        services.AddHttpClients(configuration);
        services.AddUserAgentService();
    }

    private static void AddHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        var currencyOptions = configuration.GetSection(ConfigSectionNames.CurrencyRate).Get<CurrencyRateOptions>()!;

        services.AddHttpClient("CoinGeckoClient", config =>
        {
            config.BaseAddress = new Uri(currencyOptions.CoinGeckoApiUrl);
        });

        services.AddHttpClient("MonobankClient", config =>
        {
            config.BaseAddress = new Uri(currencyOptions.MonobankApiUrl);
            // config.Re
        });
    }

    private static void AddCustomSieve(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SieveOptions>(configuration.GetSection(ConfigSectionNames.Sieve));
        services.AddScoped<ISieveCustomFilterMethods, SieveCustomFiltering>();
        services.AddScoped<ISieveProcessor, NetSieveProcessor>();
    }
}