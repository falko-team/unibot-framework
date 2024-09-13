using Talkie.Signals;

namespace Talkie.Interceptors;

internal sealed class DistinctUntilChangedSignalInterceptor<TValue>(Func<Signal, CancellationToken, TValue> select)
    : ISignalInterceptor
{
    private readonly object _locker = new();

    private bool _firstTime = true;

    private TValue _lastValue = default!;

    public InterceptionResult Intercept(Signal signal, CancellationToken cancellationToken)
    {
        for (;;)
        {
            var currentValue = select(signal, cancellationToken);
            var lastValue = _lastValue;

            if (_firstTime is false && Equals(currentValue, lastValue))
            {
                return InterceptionResult.Break();
            }

            lock (_locker)
            {
                if (_firstTime is false && Equals(_lastValue, lastValue) is false) continue;

                _firstTime = false;
                _lastValue = currentValue;

                return InterceptionResult.Continue();
            }
        }
    }
}
