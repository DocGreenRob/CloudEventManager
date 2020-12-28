using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudEventManager.Common;
using CloudEventManager.Manager.Implementation;
using CloudEventManager.Manager.Implementation.Messaging;
using CloudEventManager.Manager.Interface;
using CloudEventManager.Manager.Interface.Messaging;
using CloudEventManager.Manager.Logging;
using CloudEventManager.Models.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace SampleConsumerApiService
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();
			var cloudEventManagerConfiguration = new CloudEventManagerConfiguration();
			cloudEventManagerConfiguration.ConnectionStringHostName = "localhost";
			cloudEventManagerConfiguration.ExchangeName = "project-name.exchange";
			cloudEventManagerConfiguration.RetryConfigurationSetting = new RetrySetting
			{
				MaxAttempts = 10,
				MaxBackoffSetting = new TimeSpanSetting
				{
					Minutes = 5
				},
				MaxElapsedRetrySetting = new TimeSpanSetting
				{
					Seconds = 60
				}
			};
			cloudEventManagerConfiguration.RetryExceptionTypes = new List<Type>
			{
				typeof(System.Exception)
			};

			services.AddScoped<ITelemetryClient, TelemetryClientWrap>();
			services.AddScoped<IApplicationContext, ApplicationContext>();
			services.AddScoped<IContractResolver, ContractResolver>();
			services.AddScoped<ICloudEventManagerConfiguration>(x => cloudEventManagerConfiguration);
			services.AddScoped<ICloudEventManager, CloudEventManager.Manager.Implementation.CloudEventManager>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
