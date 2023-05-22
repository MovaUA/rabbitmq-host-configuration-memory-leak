using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MemoryLeakTest;

public class ApplicationService : BackgroundService
{
	private readonly IConsumerController consumerController;
	private readonly ILogger<ApplicationService> logger;

	public ApplicationService(
		IConsumerController consumerController,
		ILogger<ApplicationService> logger
	)
	{
		this.consumerController = consumerController;
		this.logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		CancellationTokenSource? cancelSource = null;
		Task? task = null;

		while (await Console.In.ReadLineAsync(stoppingToken) is { } command)
		{
			switch (command)
			{
				case "connect":
					if (cancelSource != null)
					{
						this.logger.LogWarning("Can't connect: already connected.");
						continue;
					}

					cancelSource = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
					task = this.consumerController.Connect(cancellationToken: cancelSource.Token);

					break;
				case "disconnect":
					if (cancelSource == null)
					{
						this.logger.LogWarning("Can't disconnect: not connected yet.");
						break;
					}

					cancelSource.Cancel();
					await task!;

					cancelSource.Dispose();
					cancelSource = null;
					task = null;

					break;
			}
		}
	}
}
