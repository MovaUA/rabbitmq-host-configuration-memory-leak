using MassTransit;
using MemoryLeakTest;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

Host
	.CreateDefaultBuilder(args)
	.ConfigureServices(ConfigureServices)
	.ConfigureLogging(logging => logging.SetMinimumLevel(LogLevel.Trace))
	.Build()
	.Run();

void ConfigureServices(
	HostBuilderContext context,
	IServiceCollection services
)
{
	services.AddSingleton<RabbitMqTransportOptions>();
	services.AddMassTransit(
		bus => bus.UsingRabbitMq()
	);

	services.AddHostedService<ApplicationService>();
	services.AddSingleton<IConsumerController, ConsumerController>();
}
