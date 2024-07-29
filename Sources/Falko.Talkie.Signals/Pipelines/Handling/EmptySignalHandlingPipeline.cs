using Talkie.Flows;
using Talkie.Signals;

namespace Talkie.Pipelines.Handling;

public sealed class EmptySignalHandlingPipeline : ISignalHandlingPipeline
{
    public static readonly EmptySignalHandlingPipeline Instance = new();

    private EmptySignalHandlingPipeline() { }

    public ValueTask TransferAsync(ISignalFlow flow, Signal signal, CancellationToken cancellationToken = default)
    {
        return ValueTask.CompletedTask;
    }
}