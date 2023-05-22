using MassTransit;
using MassTransit.Util;
using Microsoft.Extensions.Logging;

namespace MemoryLeakTest;

public class ConsumerController : IConsumerController
{
	private readonly IBus bus;
	private readonly ILogger<ConsumerController> logger;

	public ConsumerController(
		IBus bus,
		ILogger<ConsumerController> logger
	)
	{
		this.bus = bus;
		this.logger = logger;
	}

	public async Task Connect(CancellationToken cancellationToken)
	{
		var consumer = new SampleConsumer();

		var hostReceiveEndpointHandle = this.bus.ConnectReceiveEndpoint(
			queueName: "sample-message",
			configureEndpoint: endpoint => endpoint.Instance(consumer)
		);

		{
			this.logger.LogInformation($"Connecting {nameof(SampleConsumer)}...");

			await hostReceiveEndpointHandle.Ready;

			this.logger.LogInformation($"{nameof(SampleConsumer)} is connected.");
		}

		await using var _ = cancellationToken.RegisterTask(
			out var cancelRequested
		);

		{
			this.logger.LogInformation("Waiting for the cancellation is requested...");

			await cancelRequested;

			this.logger.LogInformation("The cancellation is requested.");
		}

		{
			this.logger.LogInformation($"Disconnecting {nameof(SampleConsumer)}...");

			await hostReceiveEndpointHandle.StopAsync(
				cancellationToken: CancellationToken.None
			);

			this.logger.LogInformation($"{nameof(SampleConsumer)} is disconnected.");
		}
	}
}
