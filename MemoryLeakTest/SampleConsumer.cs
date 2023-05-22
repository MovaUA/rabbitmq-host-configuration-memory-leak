using MassTransit;

namespace MemoryLeakTest;

public class SampleConsumer : IConsumer<SampleMessage>
{
	public Task Consume(ConsumeContext<SampleMessage> context)
	{
		return Task.CompletedTask;
	}
}
