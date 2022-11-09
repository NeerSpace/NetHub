﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetHub.Application.Options;
using NetHub.Core.DependencyInjection;

namespace NetHub.Infrastructure;

public static class DependencyInjection
{
	public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddLazyCache();
		services.AddHttpClients(configuration);
		services.RegisterServicesFromAssembly("NetHub.Infrastructure");
	}

	public static AuthenticationBuilder AddGoogleAuthProvider(this AuthenticationBuilder builder,
		IConfiguration configuration)
	{
		var googleOptions = configuration
			.GetSection("Google")
			.Get<GoogleOptions>();

		builder.AddGoogleOpenIdConnect(options =>
		{
			options.ClientId = googleOptions.ClientId;
			options.ClientSecret = googleOptions.ClientSecret;
		});

		return builder;
	}

	private static void AddHttpClients(this IServiceCollection services, IConfiguration configuration)
	{
		var currencyOptions = configuration
			.GetSection("CurrencyRate")
			.Get<CurrencyRateOptions>();

		services.AddHttpClient("CoinGeckoClient",
			config => { config.BaseAddress = new Uri(currencyOptions.CoinGeckoApiUrl); });
		
		services.AddHttpClient("MonobankClient",
			config =>
			{
				config.BaseAddress = new Uri(currencyOptions.MonobankApiUrl); 
				// config.Re
			});
	}
}