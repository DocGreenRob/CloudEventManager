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
			//
			services.AddControllers();
			var cloudEventManagerConfiguration = new CloudEventManagerConfiguration2();
			cloudEventManagerConfiguration.ConnectionStringConfigurationName = "conn-string-name";
			cloudEventManagerConfiguration.QueueName = "quene-name";
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
			services.AddScoped<IHttpClient, HttpClient>();
			services.AddScoped<IContractResolver, ContractResolver>();
			services.AddScoped<IQueueClientFactory, QueueClientFactory>();
			services.AddScoped<IMessageSenderFactory, MessageSenderFactory>();
			services.AddScoped<IMessagePublisherFactory, MessagePublisherFactory>();
			services.AddScoped<ICloudEventManagerConfiguration2>(x => cloudEventManagerConfiguration);
			//services.AddTransient<ICloudEventManagerConfiguration2, CloudEventManagerConfiguration2>();
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
