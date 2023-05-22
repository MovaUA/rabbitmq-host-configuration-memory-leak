namespace MemoryLeakTest;

public interface IConsumerController
{
	/// <summary>
	/// Connects a consumer of <see cref="SampleMessage"/> to the <see cref="MassTransit.IBus"/>
	/// </summary>
	/// <param name="cancellationToken">
	/// Signals to disconnect the consumer from the <see cref="MassTransit.IBus"/>
	/// </param>
	/// <returns>
	/// The task completes when connect/disconnect is completed.
	/// </returns>
	Task Connect(CancellationToken cancellationToken);
}
