using Talkie.Collections;
using Talkie.Handlers;
using Talkie.Interceptors;
using Talkie.Signals;

namespace Talkie.Pipelines;

public sealed class SignalHandlingPipelineBuilder<T> : ISignalHandlingPipelineBuilder<T> where T : Signal
{
    private readonly FrozenSequence<ISignalInterceptor> _interceptors;

    private readonly Sequence<ISignalHandler> _handlers;

    public SignalHandlingPipelineBuilder(IEnumerable<ISignalInterceptor> interceptors, IEnumerable<ISignalHandler> handlers)
    {
        _interceptors = interceptors.ToFrozenSequence();
        _handlers = handlers.ToSequence();
    }

    public SignalHandlingPipelineBuilder(IEnumerable<ISignalInterceptor> interceptors)
    {
        _interceptors = interceptors.ToFrozenSequence();
        _handlers = new Sequence<ISignalHandler>();
    }

    public ISignalHandlingPipelineBuilder<T> HandleAsync(ISignalHandler<T> handler)
    {
        _handlers.Add(handler);

        return this;
    }

    public ISignalPipeline Build()
    {
        return new SignalPipeline(_interceptors, _handlers);
    }

    public FrozenSequence<ISignalInterceptor> ToInterceptors()
    {
        return _interceptors;
    }

    public FrozenSequence<ISignalHandler> ToHandlers()
    {
        return _handlers.ToFrozenSequence();
    }
}
