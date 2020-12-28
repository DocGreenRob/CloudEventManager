using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudEventManager.Manager.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SampleConsumerApiService.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private readonly ICloudEventManager _cloudEventManager;
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<WeatherForecastController> _logger;

		public WeatherForecastController(ILogger<WeatherForecastController> logger, ICloudEventManager cloudEventManager)
		{
			_logger = logger;
			_cloudEventManager = cloudEventManager ?? throw new ArgumentNullException(nameof(cloudEventManager));
		}

		[HttpGet]
		public async Task<IEnumerable<WeatherForecast>> Get()
		{
			var rng = new Random();
			var weatherForecast = Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				Date = DateTime.Now.AddDays(index),
				TemperatureC = rng.Next(-20, 55),
				Summary = Summaries[rng.Next(Summaries.Length)]
			})
			.ToArray();

			await _cloudEventManager.ExecuteAsync<WeatherForecast>(weatherForecast.First(), "weather.update").ConfigureAwait(false);

			return weatherForecast;
		}
	}

	public class MyClass
	{
		public int MyProperty { get; set; }
	}
}
